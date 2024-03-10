using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class WeakHolyWaterSpawner : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;

        private bool IsSpawned;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void OnEnable()
        {
            SpawnItem();
        }

        public void SpawnItem()
        {
            if (ProgressManager.GetQuest(EQuest.WeakHolyWaterItem) == 0 && !IsSpawned)
            {
                var instance = ResourceManager.CreatePrefab<WeakHolyWaterItem, EQuest>(EQuest.WeakHolyWaterItem);
                instance.transform.SetParent(gameObject.transform, false);

                IsSpawned = true;
            }
        }
    }
}