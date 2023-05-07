using System;
using UnityEngine;
using Platformer.ScriptedAnimator;

namespace Platformer
{

    enum FrogStates
    {
        InitialJump,
        Idle,
        Attack,
        Rest,
        JumpRising,
        JumpFalling,
        LeaveJumpRising,
        LeaveJumpFalling
    }

    public class Frog : MonoBehaviour
    {

        private FrogAnimator FrogAnimator;

        public Action Killed = () => { };

        private Health Health;
        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;
        private IPlayer Player;
        private IResourceManager ResourceManager;

        private float DeltaY;
        private float Timer;
        private float DisappearTimer;
        private float DisappearY;
        private bool IsDamaged = false;
        private Vector3 LastPosition;
        private float VeryHighJumpForce = 750f;
        private float HighJumpForce = 600f;
        private float LowJumpForce = 325f;
        private float HorizontalSpeed = 120f;
        private float DirectionX = 1f;
        private float FirePointX;

        private int Behaviour = 0; // a counter for tracking actions, performed in row in a single direction

        [SerializeField]
        private Transform FirePoint;
        [SerializeField]
        private Collider2D SmallCollider;
        [SerializeField]
        private Collider2D BigCollider;
        // current collider
        private Collider2D Collider;

        private Enemies Bullet = Enemies.BubbleBullet;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Health = GetComponent<Health>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
            FrogAnimator = new FrogAnimator(Renderer);

            FirePointX = FirePoint.localPosition.x;

            Health.HealthChanged += OnHealthChanged;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;
            Health.Killed += OnKilled;
        }

        private void OnEnable()
        {
            LastPosition.y = transform.position.y;
            Player = CompositionRoot.GetPlayer();
            FrogAnimator.JumpRising();
        }

        private void OnHealthChanged()
        {
            IsDamaged = true;
            switch (FrogAnimator.CurrentType)
            {
                case EFrogAnimations.Idle:
                    FrogAnimator.IdleBlink();
                    break;
                case EFrogAnimations.Attack:
                    FrogAnimator.AttackBlink();
                    break;
                case EFrogAnimations.JumpRising:
                    FrogAnimator.JumpRisingBlink();
                    break;
                case EFrogAnimations.JumpFalling:
                    FrogAnimator.JumpFallingBlink();
                    break;
            }
        }

        private void OnDamageCooldownExpired()
        {
            IsDamaged = false;
            switch (FrogAnimator.CurrentType)
            {
                case EFrogAnimations.IdleBlink:
                    FrogAnimator.Idle();
                    break;
                case EFrogAnimations.AttackBlink:
                    FrogAnimator.Attack();
                    break;
                case EFrogAnimations.JumpRisingBlink:
                    FrogAnimator.JumpRising();
                    break;
                case EFrogAnimations.JumpFallingBlink:
                    FrogAnimator.JumpFalling();
                    break;
            }
        }

        private void OnKilled()
        {
            // Blood effect
            var direction = false;
            var collider = gameObject.GetComponent<Collider2D>();
            var newPosition = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
            var instance = ResourceManager.GetFromPool(GFXs.BloodBlast);

            if (DirectionX == 1)
            {
                direction = true;
            }
            if (DirectionX == -1)
            {
                direction = false;
            }
            instance.GetComponent<BloodBlast>().Initiate(newPosition, direction);

            Killed();
            gameObject.SetActive(false);
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

        public void Initiate(float direction, float disappearY, Vector2 startPosition)
        {
            DirectionX = direction;
            DisappearY = disappearY;

            IsDamaged = false;
            //face right
            CheckDirection();

            transform.position = startPosition;

            CurrentState = StateInitialJump;
        }

        private void Update()
        {
            FrogAnimator.Update();
        }

        private void FixedUpdate()
        {
            DeltaY = transform.position.y - LastPosition.y;
            LastPosition = transform.position;

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
            Rigidbody.velocity = new Vector2(DirectionX * Time.fixedDeltaTime * HorizontalSpeed, Rigidbody.velocity.y);
        }

        private void SwitchColliders(bool state)
        {
            if (!state)
            {
                Collider = BigCollider;
                BigCollider.enabled = true;
                SmallCollider.enabled = false;
            }
            else
            {
                Collider = SmallCollider;
                SmallCollider.enabled = true;
                BigCollider.enabled = false;
            }
        }

        private void SetMask(string mask)
        {
            gameObject.layer = LayerMask.NameToLayer(mask);
        }

        //All States here
        private void StateInitialJump()
        {
            SetMask("EnemyTransparent");
            SwitchColliders(false);
            FrogAnimator.JumpRising();
            Rigidbody.AddForce(new Vector2(0f, VeryHighJumpForce));
            Timer = 0f;

            CurrentState = StateJumpRising;
        }

        private void StateJumpRising()
        {
            MoveHorizontal();

            if (DeltaY < 0f)
            {
                if (!IsDamaged)
                {
                    FrogAnimator.JumpFalling();
                }
                if (IsDamaged)
                {
                    FrogAnimator.JumpFallingBlink();
                }
                
                CurrentState = StateJumpFalling;
            }
        }

        private void StateJumpFalling()
        {
            MoveHorizontal();

            //Optional Despawn
            if (transform.position.y <= DisappearY)
            {
                //splash effect
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                effect.transform.position = transform.position;

                Killed();
                gameObject.SetActive(false);
            }

            if (Grounded(LayerMasks.Solid + LayerMasks.OneWay))
            {
                if (!IsDamaged)
                {
                    FrogAnimator.Idle();
                }
                if (IsDamaged)
                {
                    FrogAnimator.IdleBlink();
                }
                
                SwitchColliders(true);
                SetMask("EnemySolid");
                Timer = 1f; // waiting for next move
                CurrentState = StateIdle;
            }
        }
        // Behaviours in Idle
        private void AttackBehaviour()
        {
            if (!IsDamaged)
            {
                FrogAnimator.Attack();
            }
            if (IsDamaged)
            {
                FrogAnimator.AttackBlink();
            }

            CurrentState = StateAttack;
            Timer = 0.5f;
        }

        private void JumpBehaviour(bool high)
        {
            if (!high)
            {
                Rigidbody.AddForce(new Vector2(0f, LowJumpForce));
            }
            if (high)
            {
                Rigidbody.AddForce(new Vector2(0f, HighJumpForce));
            }

            if (!IsDamaged)
            {
                FrogAnimator.JumpRising();
            }
            if (IsDamaged)
            {
                FrogAnimator.JumpRisingBlink();
            }

            SwitchColliders(false);
            CurrentState = StateJumpRising;
            Timer = 1f;
        }

        private void StateIdle()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0f)
            {
                var distance = transform.position.x - Player.Position.x;
                var height = Player.Position.y - transform.position.y;
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
                            Behaviour++;

                            if (Behaviour == 1)
                            {
                                AttackBehaviour();
                            }

                            if (Behaviour > 1)
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
                            Behaviour = 0;
                            DirectionX = 1f;
                            //Renderer.flipX = false;
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
                            Behaviour++;

                            if (Behaviour == 1)
                            {
                                AttackBehaviour();
                            }

                            if (Behaviour > 1)
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
                            Behaviour = 0;
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
                        if (!IsDamaged)
                        {
                            FrogAnimator.JumpRising();
                        }
                        if (IsDamaged)
                        {
                            FrogAnimator.JumpRisingBlink();
                        }

                        SwitchColliders(false);
                        SetMask("EnemyTransparent");
                        Rigidbody.AddForce(new Vector2(0f, HighJumpForce));
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
                if (!IsDamaged)
                {
                    FrogAnimator.JumpFalling();
                }
                if (IsDamaged)
                {
                    FrogAnimator.JumpFallingBlink();
                }

                CurrentState = StateLeaveJumpFalling;
            }
        }

        private void StateLeaveJumpFalling()
        {
            MoveHorizontal();

            if (transform.position.y <= DisappearY)
            {
                //splash effect
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                effect.transform.position = transform.position;

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
                var instance = ResourceManager.GetFromPool(Bullet);
                instance.transform.position = FirePoint.position;

                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= DirectionX;
                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;

                //switch to Rest
                if (!IsDamaged)
                {
                    FrogAnimator.Idle();
                }
                if (IsDamaged)
                {
                    FrogAnimator.IdleBlink();
                }

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