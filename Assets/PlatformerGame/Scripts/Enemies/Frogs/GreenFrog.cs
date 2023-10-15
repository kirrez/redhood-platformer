using System;
using UnityEngine;
using Platformer.ScriptedAnimator;

namespace Platformer
{
    public class GreenFrog : MonoBehaviour
    {
        private FrogAnimator FrogAnimator;

        public Action Killed = () => { };

        private Health Health;
        private Rigidbody2D Rigidbody;
        private IPlayer Player;
        private IResourceManager ResourceManager;

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

        private int Behaviour = 0; // a counter for tracking actions, performed in row in a single direction

        [SerializeField]
        private Collider2D SmallCollider;
        [SerializeField]
        private Collider2D BigCollider;
        // current collider
        private Collider2D Collider;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            Health = GetComponent<Health>();
            Rigidbody = GetComponent<Rigidbody2D>();
            FrogAnimator = GetComponent<FrogAnimator>();
            ResourceManager = CompositionRoot.GetResourceManager();

            Health.HealthChanged += OnHealthChanged;
            Health.Killed += OnKilled;
        }

        private void OnEnable()
        {
            LastPosition.y = transform.position.y;
            Player = CompositionRoot.GetPlayer();
            FrogAnimator.SetAnimation(FrogAnimations.JumpRise);
        }

        private void OnDisable()
        {
            Killed();
        }

        private void OnHealthChanged()
        {
            FrogAnimator.StartBlinking();
        }

        private void OnKilled()
        {
            // Blood effect
            var direction = false;
            var collider = gameObject.GetComponent<Collider2D>();
            var newPosition = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
            var instance = ResourceManager.GetFromPool(GFXs.BloodBlast);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);

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
            if (DirectionX == 1f)
            {
                FrogAnimator.SetFlip(false);
            }
            if (DirectionX == -1f)
            {
                FrogAnimator.SetFlip(true);
            }
        }

        public void Initiate(float direction, float disappearY, Vector2 startPosition)
        {
            DirectionX = direction;
            DisappearY = disappearY;

            CheckDirection();
            transform.position = startPosition;
            CurrentState = StateInitialJump;
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
            FrogAnimator.SetAnimation(FrogAnimations.JumpRise);
            Rigidbody.AddForce(new Vector2(0f, VeryHighJumpForce));
            Timer = 0f;

            CurrentState = StateJumpRising;
        }

        private void StateJumpRising()
        {
            MoveHorizontal();

            if (DeltaY < 0f)
            {
                FrogAnimator.SetAnimation(FrogAnimations.JumpFall);
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
                FrogAnimator.SetAnimation(FrogAnimations.Idle);
                SwitchColliders(true);
                SetMask("EnemySolid");
                Timer = 1f; // waiting for next move
                CurrentState = StateIdle;
            }
        }
        // Behaviours in Idle
        private void AttackBehaviour()
        {
            FrogAnimator.SetAnimation(FrogAnimations.Attack);
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

            FrogAnimator.SetAnimation(FrogAnimations.JumpRise);
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
                                if (chance > 0.8f)
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
                                if (chance > 0.8f)
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
                        FrogAnimator.SetAnimation(FrogAnimations.JumpRise);
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
                FrogAnimator.SetAnimation(FrogAnimations.JumpFall);
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
                var dynamics = CompositionRoot.GetDynamicsContainer();
                effect.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(effect);
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
                //switch to Rest
                FrogAnimator.SetAnimation(FrogAnimations.Idle);
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