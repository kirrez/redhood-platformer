using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{
    public class Bear : MonoBehaviour
    {
        public Action Killed = () => { };

        [SerializeField]
        private Collider2D RageTrigger;

        [SerializeField]
        private Transform FirePoint;
        private float FirePointX;

        [SerializeField]
        private Collider2D DamageTrigger;

        private BearAnimator Animator;
        private Collider2D Collider;
        private Collider2D DeathTrigger;

        private Health Health;
        private Rigidbody2D Rigidbody;
        private IPlayer Player;

        private Enemies Slash = Enemies.BearSlash;

        private IResourceManager ResourceManager;

        private float Timer;
        private float DirectionX = 1f;
        private float DeltaY;
        private float AttackCycle = 2f;
        private float AttackTimer;
        private Vector3 LastPosition;
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
            Animator = GetComponent<BearAnimator>();
            Collider = GetComponent<Collider2D>();

            FirePointX = FirePoint.transform.localPosition.x;

            Health.Killed += OnKilled;
            Health.HealthChanged += OnHealthChanged;
        }

        private void OnEnable()
        {
            StairTimer = 0.5f;
            SetMask("EnemySolid");
            Rigidbody.isKinematic = false;
            DamageTrigger.enabled = true;
        }

        private void OnDisable()
        {
            if (!WasSlain)
            {
                Killed();
            }
            WasSlain = false;
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

        private void SetMask(string mask)
        {
            gameObject.layer = LayerMask.NameToLayer(mask);
        }

        private void OnHealthChanged()
        {
            Animator.StartBlinking();
        }

        private void OnKilled()
        {
            CurrentState = StateDying;
        }

        private void StateDying()
        {
            var newPosition = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameEffect);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);

            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(2f, 3f));

            Animator.StopBlinking();
            Animator.SetAnimation(BearAnimations.Death);

            SetMask("EnemyInactive");
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.isKinematic = true;
            DamageTrigger.enabled = false;
            Timer = 1.35f;

            CurrentState = StateDyingFinal;
        }

        private void StateDyingFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

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
            var origin = new Vector2(Collider.bounds.center.x + Collider.bounds.extents.x * DirectionX, Collider.bounds.center.y - Collider.bounds.extents.y + 2f);//
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
                Animator.SetFlip(false);
                newPosition = new Vector2(FirePointX, FirePoint.localPosition.y);
                FirePoint.localPosition = newPosition;
            }
            if (DirectionX == -1f)
            {
                Animator.SetFlip(true);
                newPosition = new Vector2(-FirePointX, FirePoint.localPosition.y);
                FirePoint.localPosition = newPosition;
            }
        }

        private void StateInitial()
        {
            Timer = 0f;
            Animator.SetAnimation(BearAnimations.Walk);
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
                    Animator.SetAnimation(BearAnimations.Attack);
                    Timer = 0.5f;
                    CurrentState = StateAttack;

                    if (height > 0.9f)
                    {
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

                Animator.SetAnimation(BearAnimations.Idle);
                CurrentState = StateRest;
            }
        }

        private void StateRest()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Animator.SetAnimation(BearAnimations.Walk);
                CurrentState = StateRoaming;
            }
        }
    }
}