using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform SpawnPoint;

        [SerializeField]
        private float DirectionX = 1f;

        [SerializeField]
        private bool PermanentSpawn;

        private bool IsSpawned;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void OnEnable()
        {
            if (PermanentSpawn == true && IsSpawned == false)
            {
                Spawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && IsSpawned == false)
            {
                Spawn();
            }
        }

        private void OnPerished()
        {
            IsSpawned = false;
        }

        public void Spawn()
        {
            IsSpawned = true;

            var instance = ResourceManager.GetFromPool(Enemies.Zombie);
            DynamicsContainer.AddEnemy(instance);
            Zombie zombie = instance.GetComponent<Zombie>();
            zombie.Initiate(SpawnPoint.position, DirectionX);

            zombie.Perished -= OnPerished;
            zombie.Perished += OnPerished;
        }
    }
}