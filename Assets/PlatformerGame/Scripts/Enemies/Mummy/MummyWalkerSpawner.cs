using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer
{
    public class MummyWalkerSpawner : MonoBehaviour
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

            var instance = ResourceManager.GetFromPool(Enemies.MummyWalker);
            DynamicsContainer.AddEnemy(instance);
            MummyWalker mummy = instance.GetComponent<MummyWalker>();
            mummy.Initiate(SpawnPoint.position, DirectionX);

            mummy.Perished -= OnPerished;
            mummy.Perished += OnPerished;
        }

    }
}