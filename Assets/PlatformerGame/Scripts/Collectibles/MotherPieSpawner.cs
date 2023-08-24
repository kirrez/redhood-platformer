using UnityEngine;

namespace Platformer
{
    public class MotherPieSpawner : MonoBehaviour
    {
        public bool ItemPhysics;

        private bool IsSpawned;
        private EQuest Item;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            ResourceManager = CompositionRoot.GetResourceManager();

            Item = EQuest.MotherPie;
        }

        private void OnEnable()
        {
            SpawnItem();
        }

        public void SpawnItem()
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) == 4 && !IsSpawned)
            {
                var instance = ResourceManager.CreatePrefab<MotherPie, EQuest>(Item);
                instance.transform.SetParent(gameObject.transform, false);
                instance.PhysicsOn(ItemPhysics);
                IsSpawned = true;
            }
        }
    }
}