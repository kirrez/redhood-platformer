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

        private void FixedUpdate()
        {
            CurrentState();
        }

        public void Initiate(Transform newPosition, Collider2D detector)
        {
            Detector = detector;
            transform.position = newPosition.position;

            CurrentState = StateStartSpawning;
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

        private void SetMask(string mask)
        {
            Body.gameObject.layer = LayerMask.NameToLayer(mask);
        }

        private void StateStartSpawning()
        {
            Timer = Animator.PlaySpawning();
            DamageTrigger.enabled = false;
            SetMask(LayerNames.EnemyInactive);

            CurrentState = StateProgressSpawning;
        }

        private void StateProgressSpawning()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                CurrentState = StateFinishSpawning;
            }
        }

        private void StateFinishSpawning()
        {
            Animator.PlayIdle();
            Health.RefillHealth();
            DamageTrigger.enabled = true;
            SetMask(LayerNames.EnemySolid);
            Timer = 2f;

            CurrentState = StateRestAfterSpawn;
        }

        private void StateRestAfterSpawn()
        {
            Timer -= Time.fixedDeltaTime;

            CheckDetector();

            if (InsideDetector && FacePlayer)
            {
                ChangeFaceDirection();
            }

            if (Timer <= 0)
            {
                CurrentState = StateIdle;
            }
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

            AudioManager.PlaySound(ESounds.EggDrop);

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

        private void StateStartDying()
        {
            var newPosition = Body.transform.position;
            newPosition.y += 2f;
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameEffect);
            DynamicsContainer.AddMain(instance);
            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(2f, 2f));

            var amount = UnityEngine.Random.Range(8, 15);
            newPosition.y += 1f;
            for (int i = 0; i < amount; i++)
            {
                var feather = ResourceManager.GetFromPool(GFXs.FeatherParticle);
                DynamicsContainer.AddMain(feather);
                feather.GetComponent<FeatherParticle>().Initiate(newPosition);
            }

            AudioManager.PlaySound(ESounds.EagleScream);
            AudioManager.PlaySound(ESounds.EnemyDying5);

            Timer = Animator.PlayDying();

            CurrentState = StateProgressDying;
        }

        private void StateProgressDying()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                CurrentState = StateFinishDying;
            }
        }

        private void StateFinishDying()
        {
            Animator.PlaySpawning();
            Animator.Stop();
            EagleKilled?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnKilled()
        {
            CurrentState = StateStartDying;
        }

        private void OnDisable()
        {
            EagleKilled?.Invoke();
        }
    }
}