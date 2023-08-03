using UnityEngine;

namespace Platformer
{
    public class CollectibleSpawner : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private bool IsSpawned;

        //It's not very safe, but a mistake would be quiet obvious
        [SerializeField]
        private EQuest Item;

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

            if (ProgressManager.GetQuest(Item) == ConditionValue)
            {
                var instance = GameObject.Instantiate(ItemPrefab, transform.parent, false);
                instance.transform.position = transform.position;
                IsSpawned = true;
            }
        }
    }
}