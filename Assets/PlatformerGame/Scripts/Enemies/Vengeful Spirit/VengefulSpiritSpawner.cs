using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class VengefulSpiritSpawner : MonoBehaviour
    {
        // Spirit appears when trigger is reached by player. Also spirit acts in bounds of this trigger
        [SerializeField]
        private Collider2D Trigger;

        [SerializeField]
        private Transform SpawnPoint;

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

        public void Spawn()
        {
            IsSpawned = true;

            var instance = ResourceManager.GetFromPool(Enemies.VengefulSpirit);
            DynamicsContainer.AddEnemy(instance);
            VengefulSpirit spirit = instance.GetComponent<VengefulSpirit>();
            spirit.Initiate(SpawnPoint.position, Trigger);
        }

    }
}