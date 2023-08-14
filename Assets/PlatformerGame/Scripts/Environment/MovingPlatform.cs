using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MovingPlatform : BasePlatform
    {
        private Rigidbody2D Rigidbody;
        private Vector2 Velocity;
        private IPlayer Player;

        [SerializeField]
        private Collider2D ComeThroughTrigger;

        [SerializeField]
        private Transform[] Waypoints;
        private int Index;

        [SerializeField]
        private float Force = 10f;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Player = CompositionRoot.GetPlayer();
        }

        protected override void FixedUpdate()
        {
            var distance = Vector2.Distance(Waypoints[Index].position, transform.position);
            if ( distance > 0.1f)
            {
                Velocity = (Waypoints[Index].position - transform.position).normalized * Force;
                Rigidbody.velocity = Velocity;
            }

            if (distance <= 0.1f)
            {
                Index++;
                if (Index >= Waypoints.Length)
                {
                    Index = 0;
                }
            }

            if (!IsActive) return;

            Timer -= Time.fixedDeltaTime;

            if (ComeThroughTrigger.bounds.Contains(Player.Position))
            {
                Inside = true;
            }

            if (!ComeThroughTrigger.bounds.Contains(Player.Position))
            {
                Inside = false;
            }

            if (Timer <= 0f && !Inside)
            {
                gameObject.layer = (int)Layers.PlatformOneWay;
                IsActive = false;
            }
        }
    }
}