using System;
using UnityEngine;

namespace Platformer
{
    public class RedFrog : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private Collider2D DamageTrigger;

        [SerializeField]
        private Transform FirePoint;

        [SerializeField]
        private Vector2[] HigherPath;

        [SerializeField]
        private Vector2[] LowerPath;

        private FrogAnimator Animator;
        public Action Killed = () => { };

        private Health Health;
        private IPlayer Player;
        private IAudioManager AudioManager; 
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private float DeltaY;
        private float Timer;
        private float DisappearTimer;
        private float DisappearY;
        private Vector3 LastPosition;
        private float VeryHighJumpForce = 750f;
        private float HighJumpForce = 600f;
        private float LowJumpForce = 325f;
        private float HorizontalSpeed = 120f;
        private float DirectionX = 1f;
        private float FirePointX;

        private int Phase = 0; // a counter for tracking actions, performed in row in a single direction

        private PolygonCollider2D Collider;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            Health = Body.GetComponent<Health>();
            Collider = Body.GetComponent<PolygonCollider2D>();

            Animator = GetComponent<FrogAnimator>();
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();

            FirePointX = FirePoint.localPosition.x;

            Health.Killed += OnKilled;
            Health.HealthChanged += OnHealthChanged;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;
        }

        private void OnEnable()
        {
            LastPosition.y = Body.transform.position.y;
            Animator.PlayJumpRise();
            Animator.Begin();

            UnfreezeBody();

            DamageTrigger.enabled = true;
        }

        private void OnDisable()
        {
            Killed();
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

            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(2f, 2f));

            Animator.StopBlinking();
            Animator.SetNewAnimationPeriod(0.25f);
            Animator.PlayDeath();

            SetMask(LayerNames.EnemyInactive);
            Body.velocity = Vector2.zero;
            FreezeBody();
            DamageTrigger.enabled = false;
            Timer = 1.35f; //flames fully animate 3 times

            AudioManager.PlaySound(ESounds.EnemyDying5);

            CurrentState = StateDyingFinal;
        }

        private void StateDyingFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Killed();
            UnfreezeBody();

            Animator.RestoreAnimationPeriod();
            gameObject.SetActive(false);
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

        public void Initiate(float direction, float disappearY, Vector2 startPosition)
        {
            DirectionX = direction;
            DisappearY = disappearY;

            CheckDirection();
            Body.transform.position = startPosition;
            CurrentState = StateInitialJump;
        }

        private void FixedUpdate()
        {
            DeltaY = Body.transform.position.y - LastPosition.y;
            LastPosition = Body.transform.position;

            CurrentState();
        }

        public bool Grounded(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var boxSize = new Vector2(Collider.bounds.size.x, 0.1f);
            float distance = 0.1f; // Magic number, empirical
            RaycastHit2D GroundHit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, distance, mask);

            return GroundHit.collider != null;
        }

        private void MoveHorizontal()
        {
            Body.velocity = new Vector2(DirectionX * Time.fixedDeltaTime * HorizontalSpeed, Body.velocity.y);
        }

        private void SwitchColliderPaths(bool state)
        {
            if (!state)
            {
                Collider.SetPath(0, HigherPath);
            }
            else
            {
                Collider.SetPath(0, LowerPath);
            }
        }

        private void SetMask(string mask)
        {
            Body.gameObject.layer = LayerMask.NameToLayer(mask);
        }

        //All States here
        private void StateInitialJump()
        {
            SetMask(LayerNames.EnemyTransparent);
            SwitchColliderPaths(false);
            Animator.PlayJumpRise();
            Body.AddForce(new Vector2(0f, VeryHighJumpForce));
            Timer = 0f;

            CurrentState = StateJumpRising;
        }

        private void StateJumpRising()
        {
            MoveHorizontal();

            if (DeltaY < 0f)
            {
                Animator.PlayJumpFall();
                CurrentState = StateJumpFalling;
            }
        }

        private void StateJumpFalling()
        {
            MoveHorizontal();

            //Optional Despawn
            if (Body.transform.position.y <= DisappearY)
            {
                //splash effect
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                DynamicsContainer.AddMain(effect);
                effect.transform.position = Body.transform.position;

                AudioManager.PlaySound(ESounds.Splash2);

                Killed();
                gameObject.SetActive(false);
            }

            if (Grounded(LayerMasks.Solid + LayerMasks.OneWay))
            {
                Animator.PlayIdle();
                SwitchColliderPaths(true);
                SetMask(LayerNames.EnemySolid);
                Timer = 1f; // waiting for next move

                AudioManager.PlaySound(ESounds.FrogJump);

                CurrentState = StateIdle;
            }
        }
        // Behaviours in Idle
        private void AttackBehaviour()
        {
            Animator.PlayAttack();

            CurrentState = StateAttack;
            Timer = 0.5f;
        }

        private void JumpBehaviour(bool high)
        {
            if (!high)
            {
                Body.AddForce(new Vector2(0f, LowJumpForce));
            }
            if (high)
            {
                Body.AddForce(new Vector2(0f, HighJumpForce));
            }
            Animator.PlayJumpRise();

            SwitchColliderPaths(false);
            CurrentState = StateJumpRising;
            Timer = 1f;
        }

        private void StateIdle()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0f)
            {
                var distance = Body.transform.position.x - Player.Position.x;
                var height = Player.Position.y - Body.transform.position.y;
                var chance = UnityEngine.Random.Range(0f, 1f);

                // Player not too far ??
                if (Mathf.Abs(distance) < 15f)
                {
                    DisappearTimer = 0f;
                    //face left
                    if (DirectionX == -1)
                    {
                        //player's at the left side
                        if (distance > 0)
                        {
                            Phase++;

                            if (Phase == 1)
                            {
                                AttackBehaviour();
                            }

                            if (Phase > 1)
                            {
                                if (chance > 0.6f)
                                {
                                    AttackBehaviour();
                                }
                                else
                                {
                                    if (height > 1.5f)
                                    {
                                        JumpBehaviour(true);
                                    }
                                    else
                                    {
                                        JumpBehaviour(false);
                                    }
                                }
                            }
                        }

                        //player's at the right side
                        if (distance <= 0)
                        {
                            Phase = 0;
                            DirectionX = 1f;
                            CheckDirection();
                            JumpBehaviour(false);
                            return;
                        }
                    }

                    //face right
                    if (DirectionX == 1)
                    {
                        //player's at the right side
                        if (distance < 0)
                        {
                            Phase++;

                            if (Phase == 1)
                            {
                                AttackBehaviour();
                            }

                            if (Phase > 1)
                            {
                                if (chance > 0.6f)
                                {
                                    AttackBehaviour();
                                }
                                else
                                {
                                    if (height > 1.5f)
                                    {
                                        JumpBehaviour(true);
                                    }
                                    else
                                    {
                                        JumpBehaviour(false);
                                    }
                                }
                            }
                        }

                        //player's at the left side
                        if (distance >= 0)
                        {
                            Phase = 0;
                            DirectionX = -1f;
                            //Renderer.flipX = true;
                            CheckDirection();
                            JumpBehaviour(false);
                        }
                    }
                }

                if (Mathf.Abs(distance) >= 15f)
                {
                    // Leave Jump Rising..
                    // Behaviour = 0;
                    DisappearTimer += Time.fixedDeltaTime;
                    if (DisappearTimer >= 4f)
                    {
                        Animator.PlayJumpRise();
                        SwitchColliderPaths(false);
                        SetMask(LayerNames.EnemyTransparent);
                        Body.AddForce(new Vector2(0f, HighJumpForce));
                        CurrentState = StateLeaveJumpRising;
                    }
                }
            }
        }

        private void StateLeaveJumpRising()
        {
            MoveHorizontal();

            if (DeltaY < 0f)
            {
                Animator.PlayJumpFall();
                CurrentState = StateLeaveJumpFalling;
            }
        }

        private void StateLeaveJumpFalling()
        {
            MoveHorizontal();

            if (Body.transform.position.y <= DisappearY)
            {
                //splash effect
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                DynamicsContainer.AddMain(effect);
                effect.transform.position = Body.transform.position;

                AudioManager.PlaySound(ESounds.Splash2);

                Killed();
                gameObject.SetActive(false);
            }
        }

        private void StateAttack()
        {
            if (Timer > 0)
            {
                Timer -= Time.fixedDeltaTime;
            }

            if (Timer <= 0)
            {
                var instance = ResourceManager.GetFromPool(Enemies.BubbleBullet);
                DynamicsContainer.AddMain(instance);
                instance.transform.position = FirePoint.position;

                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= DirectionX;
                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;

                Animator.PlayIdle();
                AudioManager.PlaySound(ESounds.BulletShot1);

                Timer = 1f;
                CurrentState = StateRest;
            }
        }

        private void StateRest()
        {
            if (Timer > 0)
            {
                Timer -= Time.fixedDeltaTime;
            }

            if (Timer <= 0)
            {
                CurrentState = StateIdle;
            }
        }

    }
}