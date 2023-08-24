using UnityEngine;

namespace Platformer
{
    public class Bag250Spawner : MonoBehaviour
    {
        // base Bag250_00

        [SerializeField]
        [Range(0, 9)]
        private int ItemIndex;

        public bool ItemPhysics;

        private int TargetValue;
        private bool IsSpawned;
        private EQuest Item;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            ResourceManager = CompositionRoot.GetResourceManager();

            TargetValue = (int)EQuest.Bag250_00 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnEnable()
        {
            if (ProgressManager.GetQuest(Item) == 0 && !IsSpawned)
            {
                var instance = ResourceManager.CreatePrefab<Bag250, EQuest>(Item);
                instance.transform.SetParent(gameObject.transform, false);
                instance.PhysicsOn(ItemPhysics);
                IsSpawned = true;
            }
        }
    }
}