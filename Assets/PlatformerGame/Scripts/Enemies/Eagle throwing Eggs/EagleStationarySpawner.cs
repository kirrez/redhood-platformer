using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EagleStationarySpawner : MonoBehaviour
    {
        [SerializeField]
        private Collider2D Detector;

        [SerializeField]
        private float RespawnTime = 3f;

        private bool IsSpawned;
        private float Timer;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void OnEnable()
        {
            
        }

        private void FixedUpdate()
        {
            Spawn();
        }

        private void Spawn()
        {
            if (IsSpawned == true) return;

            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                IsSpawned = true;
                var instance = ResourceManager.GetFromPool(Enemies.EagleStationary);
                var eagle = instance.GetComponent<EagleStationary>();

                eagle.EagleKilled -= OnKilled;
                eagle.EagleKilled += OnKilled;

                DynamicsContainer.AddEnemy(eagle.gameObject);

                eagle.Initiate(transform, Detector);
            }
        }

        private void OnKilled()
        {
            IsSpawned = false;
            Timer = RespawnTime;
        }
    }
}