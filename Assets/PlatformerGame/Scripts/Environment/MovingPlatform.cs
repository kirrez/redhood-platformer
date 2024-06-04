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

        private EPlatformVerticalDirection Direction;
        private Vector2 LastPosition;

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

        private void OnEnable()
        {
            LastPosition = transform.position;
        }

        protected override void FixedUpdate()
        {
            var distance = Vector2.Distance(Waypoints[Index].position, transform.position);
            
            if ( distance > 0.1f)
            {
                Velocity = (Waypoints[Index].position - transform.position).normalized * Force;
                Rigidbody.velocity = Velocity;

                //platform jumps to a current waypoint if it is a teleport
                var wayPoint = Waypoints[Index].GetComponent<PlatformWaypoint>();
                if (wayPoint != null && wayPoint.IsTeleport())
                {
                    transform.position = Waypoints[Index].position;
                } 
            }

            if (distance <= 0.1f)
            {
                Index++;
                if (Index >= Waypoints.Length)
                {
                    Index = 0;
                }
            }

            CheckVerticalDirection();
            LastPosition = transform.position; // update last position


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

        private void CheckVerticalDirection()
        {
            if (LastPosition.y == transform.position.y)
            {
                Direction = EPlatformVerticalDirection.Still;
            }

            if (LastPosition.y < transform.position.y)
            {
                Direction = EPlatformVerticalDirection.Upward;
            }

            if (LastPosition.y > transform.position.y)
            {
                Direction = EPlatformVerticalDirection.Downward;
            }
        }

        public EPlatformVerticalDirection GetDirection()
        {
            return Direction;
        }
    }
}