using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Platformer
{
    public class EagleMoving : MonoBehaviour
    {
        public Action EagleKilled;

        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private Collider2D DamageTrigger;

        [SerializeField]
        private Collider2D Detector;

        [SerializeField]
        private Health Health;

        [SerializeField]
        private Transform FirePoint;

        [SerializeField]
        private float DropCooldown = 4f;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private IPlayer Player;

        private EagleAnimator Animator;
        private List<Transform> Waypoints;
        private int PointsIndex;
        private float HorizontalSpeed;

        private bool InsideDetector;
        private float Timer;

        private delegate void State();
        State Move = () => { };
        State Attack = () => { };

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
            Move();
            Attack();
        }

        public void Initiate(List<Transform> waypoints, float horizontal)
        {
            Waypoints = waypoints;
            HorizontalSpeed = horizontal;

            PointsIndex = 0;
            Body.transform.position = Waypoints[PointsIndex].position;
            DefineDirection();

            Attack = () => { };
            Move = StateStartSpawning;
        }

        private void DefineDirection()
        {
            if (PointsIndex == Waypoints.Count - 1)
            {
                PointsIndex = 0;
            }
            else
            {
                PointsIndex++;
            }

            var nextPointX = Waypoints[PointsIndex].position.x;

            if (nextPointX >= Body.transform.position.x)
            {
                Renderer.flipX = false;
            }
            else
            {
                Renderer.flipX = true;
            }
        }

        private void CheckDetector()
        {
            InsideDetector = Detector.bounds.Contains(Player.Position);
        }


        private void SetMask(string mask)
        {
            Body.gameObject.layer = LayerMask.NameToLayer(mask);
        }

        // Move States

        private void StateMoving()
        {
            var direction = (Waypoints[PointsIndex].position - Body.transform.position).normalized * HorizontalSpeed;
            
            if (Body.velocity.magnitude < 5f)
            {
                Body.AddForce(direction * Time.fixedDeltaTime, ForceMode2D.Force);
            }


            if (Vector2.Distance(Body.transform.position, Waypoints[PointsIndex].position) <= 0.3f)
            {
                Move = StateDestinationReached;
            }
        }

        private void StateDestinationReached()
        {
            DefineDirection();
            Body.velocity *= 0.35f;

            Move = StateMoving;
        }


        // Attack States

        private void StateStartSpawning()
        {
            Timer = Animator.PlaySpawning();
            DamageTrigger.enabled = false;
            SetMask(LayerNames.EnemyInactive);

            Move = StateProgressSpawning;
        }

        private void StateProgressSpawning()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Move = StateFinishSpawning;
            }
        }

        private void StateFinishSpawning()
        {
            Animator.PlayIdle();
            Health.RefillHealth();
            DamageTrigger.enabled = true;
            SetMask(LayerNames.EnemyTransparent);
            Timer = 1f;

            Move = StateMoving;
            Attack = StateRestAfterSpawn;
        }

        private void StateRestAfterSpawn()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Attack = StateIdle;
            }
        }

        private void StateIdle()
        {
            CheckDetector();

            if (InsideDetector)
            {
                Attack = StateThrowEgg;
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

            Timer = DropCooldown;
            Attack = StateRestAfterThrow;
        }

        private void StateRestAfterThrow()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Attack = StateIdle;
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

            Move = StateProgressDying;
        }

        private void StateProgressDying()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Move = StateFinishDying;
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
            Move = StateStartDying;
            Attack = () => { };
        }

        private void OnDisable()
        {
            EagleKilled?.Invoke();
        }
    }
}