using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class NPCWanderer : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D Rigidbody;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private float AnimationDelay = 0.5f;

        [SerializeField]
        private List<Transform> Waypoints;

        [SerializeField]
        private float Speed = 70f;

        private int WaypointsIndex;
        private int SpriteIndex;
        //private float Direction = 1f;
        private float AnimationTimer;
        private float Timer;
        private Vector2 Velocity;
        private bool DirectionChanged;
        private bool IdleDecision;

        delegate void StateMethod();
        StateMethod CurrentState = () => { };

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            Timer = 0f;
            WaypointsIndex = 0;
            SpriteIndex = 0;
            Renderer.sprite = Sprites[0];
            CurrentState = Decide;
        }

        private void FixedUpdate()
        {
            CurrentState();
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            AnimationTimer += Time.fixedDeltaTime;

            if (AnimationTimer >= AnimationDelay)
            {
                AnimationTimer -= AnimationDelay;

                if (SpriteIndex == Sprites.Count - 1)
                {
                    SpriteIndex = 0;
                }
                else if (SpriteIndex < Sprites.Count - 1)
                {
                    SpriteIndex++;
                }

                Renderer.sprite = Sprites[SpriteIndex];
            }
        }

        private void Decide()
        {
            var chance = UnityEngine.Random.Range(0f, 1f);

            if (chance < 0.25f)
            {
                Rigidbody.velocity = Vector2.zero;
                Timer = UnityEngine.Random.Range(1f, 4f);
                IdleDecision = false;
                CurrentState = Idle;
            }

            if (chance >= 0.25f)
            {
                Velocity = (Waypoints[WaypointsIndex].position - Rigidbody.gameObject.transform.position).normalized * Speed;
                if (Velocity.x > 0)
                {
                    Renderer.flipX = false;
                }
                if (Velocity.x < 0)
                {
                    Renderer.flipX = true;
                }

                Timer = UnityEngine.Random.Range(3f, 7f);
                DirectionChanged = false;
                CurrentState = Walk;
            }
        }

        private void Walk()
        {
            var distance = Vector2.Distance(Waypoints[WaypointsIndex].position, Rigidbody.gameObject.transform.position);
            
            if (distance > 0.15f)
            {
                Rigidbody.velocity = Velocity * Time.fixedDeltaTime;
            }

            if (distance <= 0.15f && !DirectionChanged)
            {
                Velocity = Vector2.zero;
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

        private void Idle()
        {
            if (!IdleDecision)
            {
                IdleDecision = true;
                var chance = UnityEngine.Random.Range(0f, 1f);
                if (chance <= 0.25f)
                {
                    WaypointsIndex++;
                    if (WaypointsIndex >= Waypoints.Count)
                    {
                        WaypointsIndex = 0;
                    }
                }
            }

            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                CurrentState = Decide;
            }
        }
    }
}

