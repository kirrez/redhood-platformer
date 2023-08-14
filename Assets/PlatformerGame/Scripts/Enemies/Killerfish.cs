using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class Killerfish : MonoBehaviour
    {
        public Action Killed = () => { };

        [SerializeField]
        private List<Sprite> IdleAnimation;

        [SerializeField]
        private List<Sprite> ThrustAnimation;

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
        private SpriteRenderer Renderer;
        private Rigidbody2D Rigidbody;
        private IResourceManager ResourceManager;
        private Vector2 Velocity;
        //private IPlayer Player;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Health = GetComponent<Health>();

            ResourceManager = CompositionRoot.GetResourceManager();
            //Player = CompositionRoot.GetPlayer();

            Health.Killed += OnKilled;
        }

        private void OnEnable()
        {
            CurrentState = Decide;
            CurrentAnimation = IdleAnimation;
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
            bool direction = false;

            Killed(); // KillerFishSpawner listens to this event

            var collider = gameObject.GetComponent<Collider2D>();
            var newPosition = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
            var instance = ResourceManager.GetFromPool(GFXs.BloodBlast);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance.gameObject);

            if (Rigidbody.velocity.x > 0)
            {
                direction = true;
            }
            if (Rigidbody.velocity.x < 0)
            {
                direction = false;
            }

            instance.GetComponent<BloodBlast>().Initiate(newPosition, direction);


            gameObject.SetActive(false);
        }
    }
}