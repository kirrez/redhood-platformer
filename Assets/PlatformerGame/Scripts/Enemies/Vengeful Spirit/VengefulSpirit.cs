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
        private float DirectionX;
        private float FloatingSpeed = 125f;

        public void Initiate(Vector2 newPosition, Collider2D trigger)
        {
            Body.transform.position = newPosition;
            AgressionTrigger = trigger;

            DirectionX = 1f;
            Animator.PlayFloating();
            DamageTrigger.enabled = true;

            CurrentState = StateFloating;
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
            }
        }

        private void StateFloating()
        {
            if (IsFrozen == true)
            {
                FreezeTimer -= Time.fixedDeltaTime;

                if (FreezeTimer > 0) return;

                if (FreezeTimer <= 0)
                {
                    IsFrozen = false;
                    DamageTrigger.enabled = true;
                }
            }


        }
    }
}