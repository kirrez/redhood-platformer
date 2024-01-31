using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Platformer
{
    public class EagleStationary : MonoBehaviour
    {
        public Action EagleKilled;
        
        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private Collider2D DamageTrigger;

        [SerializeField]
        private Health Health;

        [SerializeField]
        private Transform FirePoint;

        [SerializeField]
        private bool FacePlayer;

        [SerializeField]
        private float ThrowCooldown = 4f;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private IPlayer Player;

        private EagleAnimator Animator;
        private Collider2D Detector;
        private bool InsideDetector;
        private float Timer;

        private delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            AudioManager = CompositionRoot.GetAudioManager();
            ResourceManager = CompositionRoot.GetResourceManager();
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();

            Animator = GetComponent<EagleAnimator>();
            Player = CompositionRoot.GetPlayer();

            Animator.Initiate(Renderer);

            Health.Killed += OnKilled;
        }

        private void OnEnable()
        {
            
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        public void Initiate(Transform newPosition, Collider2D detector)
        {
            Detector = detector;
            transform.position = newPosition.position;

            CurrentState = StateSpawn;
        }

        private void CheckDetector()
        {
            InsideDetector = Detector.bounds.Contains(Player.Position);
        }

        private void ChangeFaceDirection()
        {
            if (Player.Position.x <= transform.position.x)
            {
                Renderer.flipX = true;
            }

            if (Player.Position.x > transform.position.x)
            {
                Renderer.flipX = false;
            }
        }

        private void StateSpawn()
        {
            Animator.PlayIdle();
            Health.RefillHealth();

            CurrentState = StateIdle;
        }

        private void StateIdle()
        {
            CheckDetector();

            if (InsideDetector)
            {
                if (FacePlayer)
                {
                    ChangeFaceDirection();
                }

                CurrentState = StateThrowEgg;
            }
        }

        private void StateThrowEgg()
        {
            var instance = ResourceManager.GetFromPool(Enemies.EggBomb);
            DynamicsContainer.AddEnemy(instance);
            var egg = instance.GetComponent<EggBomb>();
            egg.transform.position = FirePoint.position;
            egg.Throw();

            Timer = ThrowCooldown;
            CurrentState = StateRestAfterThrow;
        }

        private void StateRestAfterThrow()
        {
            Timer -= Time.fixedDeltaTime;

            if (Player.Position.x <= transform.position.x)
            {
                Renderer.flipX = true;
            }

            if (Player.Position.x > transform.position.x)
            {
                Renderer.flipX = false;
            }

            if (Timer <= 0)
            {
                CurrentState = StateIdle;
            }
        }

        //private void StateWaitRespawn()
        //{
        //    Timer -= Time.fixedDeltaTime;

        //    if (Timer <= 0)
        //    {
        //        CurrentState = StateSpawn;
        //    }
        //}

        private void OnKilled()
        {
            Animator.Stop();
            EagleKilled?.Invoke();

            gameObject.SetActive(false);
            //Timer = RespawnTime;
            //CurrentState = StateWaitRespawn;
        }

        private void OnDisable()
        {
            EagleKilled?.Invoke();
        }
    }
}