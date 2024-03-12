using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HolyWaterWave : MonoBehaviour
    {
        [SerializeField]
        private int ParticleAmount;

        [SerializeField]
        private float ParticleForce;

        [SerializeField]
        private float EffectDuration;

        [SerializeField]
        private CircleCollider2D Collider;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;

        private Vector2 SpawnPosition;

        private float Timer;
        private float FreezeDuration;

        RaycastHit2D[] Hits;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        public void Initiate(Vector2 spawnPosition, float duration, float radius)
        {
            SpawnPosition = spawnPosition;
            FreezeDuration = duration;
            Collider.radius = radius;

            Timer = EffectDuration;
            Collider.enabled = true;

            LaunchParticles();

            CurrentState = StateFreeze;
        }

        private void LaunchParticles()
        {
            Collider.transform.position = SpawnPosition;

            for (int i = 0; i < ParticleAmount; i++)
            {
                var particle = ResourceManager.GetFromPool(GFXs.HolyStarParticle);
                particle.transform.position = SpawnPosition;
                var rigidbody = particle.GetComponent<Rigidbody2D>();
                rigidbody.AddForce(Random.insideUnitCircle.normalized * ParticleForce, ForceMode2D.Impulse);
                rigidbody.angularVelocity = Random.Range(100f, 200f);

                DynamicsContainer.AddMain(particle);
            }
        }

        private void StateFreeze()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0 )
            {
                Collider.enabled = false;
                CurrentState = StateRest;
            }

            Hits = Physics2D.CircleCastAll(Collider.bounds.center, Collider.radius, new Vector2(0f, 0f));

            if (Hits.Length > 0) 
                foreach (var hit in Hits)
                {
                    Undead target = hit.collider.GetComponentInParent<Undead>();
                    
                    if (target != null)
                    {
                        target.Freezing(FreezeDuration);
                    }
                }
        }

        private void StateRest()
        {
            gameObject.SetActive(false);
        }
    }
}