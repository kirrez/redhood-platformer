using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{

    public class EvilBear : MonoBehaviour
    {
        public Action Killed = () => { };

        [SerializeField]
        private Rigidbody2D Body;

        //Test purpose
        [SerializeField]
        private Text RageText;

        [SerializeField]
        private Collider2D RageTrigger;

        [SerializeField]
        private Transform FirePoint;
        private float FirePointX;

        [SerializeField]
        private Collider2D DamageTrigger;

        private BearAnimator Animator;
        private Collider2D Collider;

        private Health Health;
        private IPlayer Player;

        private Enemies Slash = Enemies.BearSlash;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;

        private float Timer;
        private float DirectionX = 1f;
        private float DeltaY;
        private float Rage;
        private float AttackCycle = 2f;
        private float AttackTimer;
        private Vector3 LastPosition;
        private bool WasSlain = false;

        private float HorizontalSpeed = 100f;
        //private float FastHorizontalSpeed = 275f;
        private float JumpForce = 370f;
        private float StairTimer;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();
            Animator = GetComponent<BearAnimator>();

            Health = Body.GetComponent<Health>();
            Collider = Body.GetComponent<Collider2D>();

            FirePointX = FirePoint.transform.localPosition.x;
            Health.Killed += OnKilled;
            Health.HealthChanged += OnHealthChanged;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;
        }

        private void OnEnable()
        {
            StairTimer = 0.5f;
            SetMask(LayerNames.EnemySolid);
            UnfreezeBody();

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
            DeltaY = Body.transform.position.y - LastPosition.y;
            LastPosition = Body.transform.position;

            CurrentState();
        }

        private void SetMask(string mask)
        {
            Body.gameObject.layer = LayerMask.NameToLayer(mask);
        }

        private void OnHealthChanged()
        {
            Animator.StartBlinking();
            SetMask(LayerNames.EnemyInactive);
            DamageTrigger.enabled = false;

            AudioManager.PlaySound(ESounds.EnemyDamage2);
        }

        private void OnDamageCooldownExpired()
        {
            if (Health.GetHitPoints > 0)
            {
                SetMask(LayerNames.EnemySolid);
                DamageTrigger.enabled = true;
            }
        }

        private void OnKilled()
        {
            CurrentState = StateDying;
        }

        private void FreezeBody()
        {
            Body.isKinematic = true;
        }

        private void UnfreezeBody()
        {
            Body.isKinematic = false;
        }

        private void StateDying()
        {
            var newPosition = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameEffect);
            DynamicsContainer.AddMain(instance);

            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(2f, 3f));

            Animator.StopBlinking();
            Animator.SetAnimation(BearAnimations.Death);

            SetMask(LayerNames.EnemyInactive);
            Body.velocity = Vector2.zero;
            FreezeBody();
            DamageTrigger.enabled = false;
            Timer = 1.35f;

            AudioManager.PlaySound(ESounds.EnemyDying5);
            AudioManager.PlaySound(ESounds.VoiceBeastUuu);

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

        public void Initiate(float direction, Vector2 startPosition)
        {
            DirectionX = direction;
            CheckDirection();
            Body.transform.position = startPosition;

            CurrentState = StateInitial;
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
            Body.velocity = new Vector2(DirectionX * Time.fixedDeltaTime * HorizontalSpeed, Body.velocity.y);

            //delay between stair rises
            StairTimer -= Time.fixedDeltaTime;

            if (CheckWall(LayerMasks.Ground + LayerMasks.EnemyBorder))
            {
                DirectionX *= -1;
                CheckDirection();
            }

            if (CheckStair(LayerMasks.Ground) && StairTimer <= 0)
            {
                Body.velocity = new Vector2(Body.velocity.x, Body.velocity.y + 6.5f);
                StairTimer = 0.5f;
            }

            var distance = Player.Position.x - Body.transform.position.x;
            var height = Player.Position.y - Body.transform.position.y;

            if (AttackTimer > 0)
            {
                AttackTimer -= Time.fixedDeltaTime;
            }

            if (CheckRageZone(distance) && Rage < 1f)
            {
                Rage += Time.fixedDeltaTime / (Mathf.Abs(distance * 0.5f));
                Mathf.Clamp01(Rage);
                RageText.text = Rage.ToString("0.00");
            }
            else if (Rage > 0f)
            {
                Rage -= Time.fixedDeltaTime / 10f;
                Mathf.Clamp01(Rage);
                RageText.text = Rage.ToString("0.00");
            }

            if (CheckRageZone(distance) && Mathf.Abs(distance) < 3 && AttackTimer <= 0f)
            {
                AttackTimer = AttackCycle;
                var chance = UnityEngine.Random.Range(0f, 1f);

                // magic number
                if (chance > 0.4f)
                {
                    Rage = 0f;
                    RageText.text = Rage.ToString("0.00");
                    Animator.SetAnimation(BearAnimations.Attack);
                    Timer = 0.5f;
                    CurrentState = StateAttack;

                    if (height > 0.9f)
                    {
                        Body.AddForce(new Vector2(0f, JumpForce));
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
                DynamicsContainer.AddMain(instance);
                instance.transform.position = FirePoint.position;
                instance.GetComponent<BearSlash>().SetHitDirection(DirectionX);

                AudioManager.PlaySound(ESounds.Hit1);

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