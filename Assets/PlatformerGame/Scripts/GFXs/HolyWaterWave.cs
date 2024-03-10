using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HolyWaterWave : MonoBehaviour
    {
        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;

        private Vector2 SpawnPosition;
        private int Amount;
        private float Amplitude;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        public void Initiate(Vector2 spawnPosition, int amount, float amplitude)
        {
            SpawnPosition = spawnPosition;
            Amount = amount;
            Amplitude = amplitude;
        }

        public void LaunchParticles()
        {
            for (int i = 0; i < Amount; i++)
            {
                var particle = ResourceManager.GetFromPool(GFXs.HolyStarParticle);
                particle.transform.position = SpawnPosition;
                var rigidbody = particle.GetComponent<Rigidbody2D>();
                rigidbody.AddForce(Random.insideUnitCircle.normalized * Amplitude, ForceMode2D.Impulse);
                rigidbody.angularVelocity = Random.Range(100f, 200f);

                DynamicsContainer.AddMain(particle);
            }
        }


    }
}