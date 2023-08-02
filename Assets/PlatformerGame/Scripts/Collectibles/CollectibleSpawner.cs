using UnityEngine;

namespace Platformer
{
    public class CollectibleSpawner : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private bool IsSpawned;
        private EQuest Item;

        [SerializeField]
        private int ItemIndex;

        [SerializeField]
        private int ConditionValue = 0;

        [SerializeField]
        private GameObject ItemPrefab;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnEnable()
        {
            if (IsSpawned) return;

            Item = (EQuest)ItemIndex;

            if (ProgressManager.GetQuest(Item) == ConditionValue)
            {
                var instance = GameObject.Instantiate(ItemPrefab, transform.parent, false);
                instance.transform.position = transform.position;
                IsSpawned = true;
            }
        }
    }
}