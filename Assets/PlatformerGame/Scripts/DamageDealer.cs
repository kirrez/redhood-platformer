using System;
using UnityEngine;

namespace Platformer
{
    public class DamageDealer : MonoBehaviour
    {
        public Action Destructed;

        public Vector2 Velocity
        {
            get { return new Vector2(HorizontalVelocity, VerticalVelocity); }
        }

        [SerializeField]
        private int Damage;

        [SerializeField]
        private float HorizontalVelocity;

        [SerializeField]
        private float VerticalVelocity;

        [SerializeField]
        private int TargetType; // 0-Player, 1-Enemy, etc

        // enemies with durability > 0 instantly die on hit with player
        [SerializeField]
        private int Durability = 0; // 0 - indestructible, > 0 - destructed after number of hits
        private int CurrentDurability;

        [SerializeField]
        private bool CanPenetrateWalls = false;

        [SerializeField]
        private bool Moving = false;

        [SerializeField]
        private float HitDirection;
        private float LastX;

        

        private void Awake()
        {
            //DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            //ResourceManager = CompositionRoot.GetResourceManager();
        }

        public void SetHitDirection(float direction)
        {
            HitDirection = direction;
        }

        private void CheckDestruction()
        {
            Health myHealth = gameObject.GetComponent<Health>();
            if (myHealth != null)
            {
                // enemy with "health" and not able to penetrate walls instantly dies on hit with solid ground..
                // the same goes if LifeTime drains to 0
                // the same if enemy with durability > 0 (and CurrentDurability < 0) hits player
                myHealth.Killed();

            }
            else
            {
                Destructed?.Invoke();
                this.gameObject.SetActive(false); // pooled in RM
            }
        }

        private void OnEnable()
        {
            CurrentDurability = Durability;
        }

        private void FixedUpdate()
        {
            if (Moving)
            {
                if (transform.position.x > LastX)
                {
                    HitDirection = 1f;
                }

                if (transform.position.x < LastX)
                {
                    HitDirection = -1f;
                }

                if (transform.position.x == LastX)
                {
                    HitDirection = 0f;
                }

                LastX = transform.position.x;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // weapon cannot move through "Ground" layer (#6) [ and "GroundSlope" #15 ] if CanPenetrateWalls is false
            if (((collision.gameObject.layer == (int)Layers.Ground) || (collision.gameObject.layer == (int)Layers.GroundSlope)) && (CanPenetrateWalls == false))
            {
                CheckDestruction();
            }

            var health = collision.gameObject.GetComponent<Health>();

            if (health != null)
            {
                if (health.GetCharacterType() == TargetType)
                {
                    health.ReceiveDamage(Damage, HitDirection);
                    CurrentDurability -= 1;
                    if ((Durability > 0) && (CurrentDurability <= 0))
                    {
                        CheckDestruction();
                    }
                }
            }
        }
    }
}