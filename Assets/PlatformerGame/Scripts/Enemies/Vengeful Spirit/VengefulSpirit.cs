using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class VengefulSpirit : Undead
    {
        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private Collider2D DamageTrigger;

        private VengefulSpiritAnimator Animator;
        private Collider2D AgressionTrigger; // delivered by spawner

        private delegate void State();
        State CurrentState = () => { };

        private IPlayer Player;
        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private float FreezeTimer;
        private bool IsFrozen;

        private float Timer;
        //private float DirectionX;
        private Vector2 OldVelocity;
        private float RoamingForce = 6f;

        public void Initiate(Vector2 newPosition, Collider2D trigger)
        {
            Body.transform.position = newPosition;
            AgressionTrigger = trigger;

            //DirectionX = 1f;
            DamageTrigger.enabled = true;

            Animator.PlayCautious();
            Animator.Begin();

            Timer = 0.75f;
            Body.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
            CurrentState = StateAppearanceStart;
        }

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();
            
            Animator = GetComponent<VengefulSpiritAnimator>();
            
            Freezing -= OnStartFreezing;
            Freezing += OnStartFreezing;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void OnStartFreezing(float duration)
        {
            if (IsFrozen == false)
            {
                IsFrozen = true;
                DamageTrigger.enabled = false;
                FreezeTimer = duration;
                Animator.StartFreeze(duration);
                OldVelocity = Body.velocity;
                Body.velocity = Vector2.zero;
            }
        }

        private void FlyRandomDirection()
        {
            Vector2 direction = UnityEngine.Random.insideUnitCircle;

            if (direction.x > 0) Animator.SetFlip(false);
            if (direction.x < 0) Animator.SetFlip(true);

            Body.AddForce(direction * RoamingForce, ForceMode2D.Impulse);
        }

        private void FlyToCenter()
        {
            Vector2 direction = ((Vector2)AgressionTrigger.bounds.center - Body.position).normalized;

            if (direction.x > 0) Animator.SetFlip(false);
            if (direction.x < 0) Animator.SetFlip(true);

            Body.AddForce(direction * RoamingForce, ForceMode2D.Impulse);
        }

        private void FlyToPlayer()
        {
            var targetPosition = Player.Position;
            targetPosition.x -= 1.5f;
            Vector2 direction = ((Vector2)targetPosition - Body.position).normalized;

            if (direction.x > 0) Animator.SetFlip(false);
            if (direction.x < 0) Animator.SetFlip(true);

            Body.AddForce(direction * RoamingForce, ForceMode2D.Impulse);
        }

        private void StateAppearanceStart()
        {
            //no freezing here
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Body.velocity = Vector2.zero;
            Body.AddForce(new Vector2(0f, -5f), ForceMode2D.Impulse);
            Timer = 0.75f;
            CurrentState = StateAppearanceFinal;
        }

        private void StateAppearanceFinal()
        {
            //no freezing here
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Body.velocity = Vector2.zero;
            Animator.PlayFloating();
            Timer = 1f;
            CurrentState = StateDecide;
        }

        private void StateDecide()
        {
            // Freeze part in every state
            if (IsFrozen == true)
            {
                FreezeTimer -= Time.fixedDeltaTime;

                if (FreezeTimer > 0) return;

                if (FreezeTimer <= 0)
                {
                    IsFrozen = false;
                    DamageTrigger.enabled = true;
                    Body.velocity = OldVelocity;
                }
            }
            //

            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            if (AgressionTrigger.bounds.Contains(Body.position) == false)
            {
                FlyToCenter();
            }
            else
            {
                if (AgressionTrigger.bounds.Contains(Player.Position) == true)
                {
                    var chance = UnityEngine.Random.value;
                    if (chance <= 0.7f)
                    {
                        Animator.PlayPursuing();
                        AudioManager.PlaySound(ESounds.Flames2);
                        FlyToPlayer();
                    }
                    else
                    {
                        FlyRandomDirection();
                    }
                }
                else
                {
                    FlyRandomDirection();
                }
            }
            
            Timer = UnityEngine.Random.Range(2f, 3.5f);
            CurrentState = StateFloating;
        }

        private void StateRest()
        {
            // Freeze part in every state
            if (IsFrozen == true)
            {
                FreezeTimer -= Time.fixedDeltaTime;

                if (FreezeTimer > 0) return;

                if (FreezeTimer <= 0)
                {
                    IsFrozen = false;
                    DamageTrigger.enabled = true;
                    Body.velocity = OldVelocity;
                }
            }
            //

            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 0.2f;
            CurrentState = StateDecide;
        }

        private void StateFloating()
        {
            // Freeze part in every state
            if (IsFrozen == true)
            {
                FreezeTimer -= Time.fixedDeltaTime;

                if (FreezeTimer > 0) return;

                if (FreezeTimer <= 0)
                {
                    IsFrozen = false;
                    DamageTrigger.enabled = true;
                    Body.velocity = OldVelocity;
                }
            }
            //

            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 0.5f;
            Animator.PlayFloating();
            CurrentState = StateRest;
        }

    }
}