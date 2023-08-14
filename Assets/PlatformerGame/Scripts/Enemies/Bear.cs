using Platformer.ScriptedAnimator;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{
    enum BearStates
    {
        Initial,
        Roaming,
        Attack,
        Rest
    }

    public class Bear : MonoBehaviour
    {
        public Action Killed = () => { };

        [SerializeField]
        private Collider2D RageTrigger;

        [SerializeField]
        private Transform FirePoint;
        private float FirePointX;

        private BearAnimator BearAnimator;
        private Collider2D Collider;
        private Collider2D DeathTrigger;

        private Health Health;
        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;
        private IPlayer Player;

        private Enemies Slash = Enemies.BearSlash;

        private IResourceManager ResourceManager;

        private float Timer;
        private float DirectionX = 1f;
        private float DeltaY;
        private float AttackCycle = 2f;
        private float AttackTimer;
        private Vector3 LastPosition;
        private bool IsDamaged = false;
        private bool WasSlain = false; 
        private float HorizontalSpeed = 100f;
        private float JumpForce = 350f;
        private float StairTimer;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Player = CompositionRoot.GetPlayer();
            Health = GetComponent<Health>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
            BearAnimator = new BearAnimator(Renderer);
            Collider = GetComponent<Collider2D>();

            FirePointX = FirePoint.transform.localPosition.x;

            Health.HealthChanged += OnHealthChanged;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;
            Health.Killed += OnKilled;
        }

        private void OnEnable()
        {
            BearAnimator.Idle();
        }

        private void OnDisable()
        {
            if (!WasSlain)
            {
                Killed();
            }
            WasSlain = false;
        }

        private void Update()
        {
            BearAnimator.Update();
        }

        private void FixedUpdate()
        {
            DeltaY = transform.position.y - LastPosition.y;
            LastPosition = transform.position;

            if (DeathTrigger != null && DeathTrigger.bounds.Contains(transform.position))
            {
                OnKilled();
            }

            CurrentState();
        }

        //Full animations cannot be switched )

        private void OnHealthChanged()
        {
            IsDamaged = true;
            switch (BearAnimator.CurrentType)
            {
                case EBearAnimations.Idle:
                    BearAnimator.IdleBlink();
                    break;
                case EBearAnimations.Walk:
                    BearAnimator.WalkBlink();
                    break;
            }
        }

        private void OnDamageCooldownExpired()
        {
            IsDamaged = false;
            switch (BearAnimator.CurrentType)
            {
                case EBearAnimations.IdleBlink:
                    BearAnimator.Idle();
                    break;
                case EBearAnimations.WalkBlink:
                    BearAnimator.Walk();
                    break;
            }
        }

        private void OnKilled()
        {
            var newPosition = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y);
            var instance = ResourceManager.GetFromPool(GFXs.BombBlast);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);
            instance.GetComponent<BombBlast>().Initiate(newPosition);

            Killed();
            WasSlain = true;
            gameObject.SetActive(false);
        }

        public void Initiate(float direction, Vector2 startPosition, Collider2D deathTrigger = null)
        {
            DirectionX = direction;
            CheckDirection();
            transform.position = startPosition;
            DeathTrigger = deathTrigger;

            CurrentState = StateInitial;
        }

        private void MoveHorizontal()
        {
            Rigidbody.velocity = new Vector2(DirectionX * Time.fixedDeltaTime * HorizontalSpeed, Rigidbody.velocity.y);

            //delay between stair rises
            StairTimer -= Time.fixedDeltaTime;

            if (CheckWall(LayerMasks.Ground))
            {
                DirectionX *= -1;
                CheckDirection();
            }

            if (CheckStair(LayerMasks.Ground) && StairTimer <= 0)
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y + 6.5f);
                StairTimer = 0.5f;
            }

        }

        // insurmauntable obstacle
        private bool CheckWall(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x + Collider.bounds.extents.x * DirectionX, Collider.bounds.center.y - Collider.bounds.extents.y + 1.5f);//
            var boxSize = new Vector2(0.2f, 0.8f);

            float distance = 0.1f; // Magic number, empirical

            RaycastHit2D WallHit = Physics2D.BoxCast(origin, boxSize, 0f, new Vector2(1f * DirectionX, 0f), distance, mask);

            return WallHit.collider != null;
        }

        // surmountable obstacle
        private bool CheckStair(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x + Collider.bounds.extents.x * DirectionX, Collider.bounds.center.y - Collider.bounds.extents.y + 0.5f);
            var boxSize = new Vector2(0.2f, 0.8f);

            float distance = 0.1f; // Magic number, empirical

            RaycastHit2D StairHit = Physics2D.BoxCast(origin, boxSize, 0f, new Vector2(1f * DirectionX, 0f), distance, mask);

            return StairHit.collider != null;
        }

        private void CheckDirection()
        {
            Vector2 newPosition;
            if (DirectionX == 1f)
            {
                Renderer.flipX = false;
                newPosition = new Vector2(FirePointX, FirePoint.localPosition.y);
                FirePoint.localPosition = newPosition;
            }
            if (DirectionX == -1f)
            {
                Renderer.flipX = true;
                newPosition = new Vector2(-FirePointX, FirePoint.localPosition.y);
                FirePoint.localPosition = newPosition;
            }
        }

        private void StateInitial()
        {
            IsDamaged = false;
            Timer = 0f;
            BearAnimator.Walk();
            CurrentState = StateRoaming;
        }

        private bool CheckRageZone(float distance)
        {
            if (RageTrigger.bounds.Contains(Player.Position))
            {
                if (Mathf.Sign(distance) == DirectionX)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private void StateRoaming()
        {
            MoveHorizontal();

            var distance = Player.Position.x - transform.position.x;
            var height = Player.Position.y - transform.position.y;

            if (AttackTimer > 0)
            {
                AttackTimer -= Time.fixedDeltaTime;
            }

            if (CheckRageZone(distance) && Mathf.Abs(distance) < 3 && AttackTimer <= 0f)
            {
                AttackTimer = AttackCycle;
                var chance = UnityEngine.Random.Range(0f, 1f);

                // magic number
                if (chance > 0.4f)
                {
                    if (!IsDamaged)
                    {
                        BearAnimator.Attack();
                    }
                    else
                    {
                        BearAnimator.AttackBlink();
                    }

                    Timer = 0.5f;
                    CurrentState = StateAttack;

                    if (height > 0.9f)
                    {
                        //Rigidbody.AddForce(new Vector2(0, JumpForce));
                        Rigidbody.AddForce(new Vector2(Rigidbody.velocity.x, JumpForce));
                    }
                }
            }
        }

        private void StateAttack()
        {
            Timer -= Time.fixedDeltaTime;
            
            if (Timer <= 0)
            {
                var instance = ResourceManager.GetFromPool(Slash);
                var dynamics = CompositionRoot.GetDynamicsContainer();
                instance.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(instance);
                instance.transform.position = FirePoint.position;

                instance.GetComponent<BearSlash>().SetHitDirection(DirectionX);

                Timer = 2f;

                //BearAnimator.Idle();
                if (!IsDamaged)
                {
                    BearAnimator.Idle();
                }
                else
                {
                    BearAnimator.IdleBlink();
                }

                CurrentState = StateRest;
            }
        }

        private void StateRest()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                //BearAnimator.Walk();
                if (!IsDamaged)
                {
                    BearAnimator.Walk();
                }
                else
                {
                    BearAnimator.WalkBlink();
                }

                CurrentState = StateRoaming;
            }
        }
    }
}