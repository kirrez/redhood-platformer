using UnityEngine;

namespace Platformer
{
    public class MushroomSpawner : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 2)]
        private int ItemIndex;

        private int TargetValue;
        private bool IsSpawned;
        private EQuest Item;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            ResourceManager = CompositionRoot.GetResourceManager();

            TargetValue = (int)EQuest.Mushroom0 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnEnable()
        {
            if (ProgressManager.GetQuest(Item) == 0 && !IsSpawned)
            {
                var instance = ResourceManager.CreatePrefab<Mushroom, EQuest>(Item);
                instance.transform.position = transform.position;
                IsSpawned = true;
            }
        }
    }
}