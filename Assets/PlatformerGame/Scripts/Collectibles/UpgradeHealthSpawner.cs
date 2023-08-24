using UnityEngine;

namespace Platformer
{
    public class UpgradeHealthSpawner : MonoBehaviour
    {
        public bool ItemPhysics;

        private EQuest Item;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            ResourceManager = CompositionRoot.GetResourceManager();

            Item = EQuest.UpgradeHealth;
        }

        private void OnEnable()
        {
            SpawnItem();
        }

        public void SpawnItem()
        {
            if (ProgressManager.GetQuest(Item) == 1)
            {
                var instance = ResourceManager.CreatePrefab<UpgradeHealth, EQuest>(Item);
                instance.transform.SetParent(gameObject.transform, false);
                instance.PhysicsOn(ItemPhysics);
            }
        }
    }
}