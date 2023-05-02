using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MovingPlatform : MonoBehaviour
    {
        private Rigidbody2D Rigidbody;
        private Vector2 Velocity;
        private IPlayer Player;

        [SerializeField]
        private Collider2D Collider;

        [SerializeField]
        private Transform[] Waypoints;
        private int Index;

        [SerializeField]
        private float Force = 10f;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            Player = CompositionRoot.GetPlayer();
        }

        private void FixedUpdate()
        {
            Velocity = (Waypoints[Index].position - transform.position).normalized * Force;
            Rigidbody.velocity = Velocity;

            if (Vector2.Distance(Waypoints[Index].position, transform.position) < 0.1f)
            {
                Index++;
                if (Index >= Waypoints.Length)
                {
                    Index = 0;
                }

                //if (Collider.bounds.Contains(Player.Position))
                //{
                //    Player.StickToPlatform();
                //}
            }
        }
    }
}