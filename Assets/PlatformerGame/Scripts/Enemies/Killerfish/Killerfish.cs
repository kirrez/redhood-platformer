using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class Killerfish : MonoBehaviour
    {
        public Action Killed = () => { };

        [SerializeField]
        private Collider2D DamageTrigger;

        [SerializeField]
        private List<Sprite> IdleAnimation;

        [SerializeField]
        private List<Sprite> ThrustAnimation;

        [SerializeField]
        private List<Sprite> DeathAnimation;

        private List<Sprite> CurrentAnimation;

        private int AnimationIndex;
        private float AnimationDelay = 0.2f;
        private float AnimationTimer;

        private List<Transform> Waypoints;
        private int WaypointsIndex;
        private float Force;
        private float Timer;
        private bool DirectionChanged;

        private Health Health;
        private Collider2D Collider;
        private SpriteRenderer Renderer;
        private Rigidbody2D Rigidbody;
        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private Vector2 Velocity;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            Health = GetComponent<Health>();

            AudioManager = CompositionRoot.GetAudioManager();
            ResourceManager = CompositionRoot.GetResourceManager();
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();

            Health.Killed += OnKilled;
        }

        private void OnEnable()
        {
            CurrentState = Decide;
            CurrentAnimation = IdleAnimation;
            gameObject.layer = LayerMask.NameToLayer("EnemyTransparent");

            DamageTrigger.enabled = true;
        }

        private void OnDisable()
        {
            Killed();
        }

        private void FixedUpdate()
        {
            CurrentState();
            PlayAnimation();
        }

        public void Initiate(Vector2 startPosition, List<Transform> waypoints)
        {
            transform.position = startPosition;
            Waypoints = waypoints;
        }

        private void PlayAnimation()
        {
            AnimationTimer += Time.fixedDeltaTime;

            if (AnimationTimer >= AnimationDelay)
            {
                AnimationTimer -= AnimationDelay;

                if (AnimationIndex == CurrentAnimation.Count - 1)
                {
                    AnimationIndex = 0;
                }
                else if (AnimationIndex < CurrentAnimation.Count - 1)
                {
                    AnimationIndex++;
                }

                Renderer.sprite = CurrentAnimation[AnimationIndex];
            }
        }

        private void Decide()
        {
            var chance = UnityEngine.Random.Range(0f, 1f);

            if (chance < 0.35f)
            {
                Timer = UnityEngine.Random.Range(0.75f, 1.4f);
                AnimationIndex = 0;
                CurrentAnimation = IdleAnimation;
                CurrentState = Idle;
            }

            if (chance >= 0.35f)
            {
                //Thrust towards next way point
                Force = UnityEngine.Random.Range(1.5f, 4f);
                Velocity = (Waypoints[WaypointsIndex].position - transform.position).normalized * Force;

                if (Velocity.x > 0)
                {
                    Renderer.flipX = false;
                }
                if (Velocity.x < 0)
                {
                    Renderer.flipX = true;
                }

                Rigidbody.AddForce(Velocity, ForceMode2D.Impulse);
                Timer = 2f;
                DirectionChanged = false;

                AnimationIndex = 0;
                CurrentAnimation = ThrustAnimation;
                CurrentState = Thrust;
            }
            
        }

        private void Idle()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                CurrentState = Decide;
            }
        }

        private void Thrust()
        {
            var distance = Vector2.Distance(Waypoints[WaypointsIndex].position, transform.position);

            if (distance <= 0.15f && !DirectionChanged)
            {
                Rigidbody.velocity = Vector2.zero;
                WaypointsIndex++;
                if (WaypointsIndex >= Waypoints.Count)
                {
                    WaypointsIndex = 0;
                }
                DirectionChanged = true;
            }

            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                CurrentState = Decide;
            }
        }

        private void OnKilled()
        {
            CurrentState = StateDying;
        }

        private void StateDying()
        {
            var newPosition = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameEffect);
            instance.transform.SetParent(DynamicsContainer.Main, false);
            DynamicsContainer.AddMain(instance);

            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(3f, 2f));

            AnimationIndex = 0;
            CurrentAnimation = DeathAnimation;

            gameObject.layer = LayerMask.NameToLayer("EnemyInactive");
            Rigidbody.velocity = Vector2.zero;


            Rigidbody.isKinematic = true;
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
            Rigidbody.isKinematic = false;

            gameObject.SetActive(false);
        }
    }
}