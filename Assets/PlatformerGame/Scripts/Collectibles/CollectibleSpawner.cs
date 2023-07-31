using UnityEngine;

namespace Platformer
{
    public class CollectibleSpawner : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private bool IsSpawned;

        [SerializeField]
        private EQuest Item;

        [SerializeField]
        private GameObject ItemPrefab;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnEnable()
        {
            if (IsSpawned) return;

            if (ProgressManager.GetQuest(Item) == 0)
            {
                var instance = GameObject.Instantiate(ItemPrefab, transform.parent, false);
                instance.transform.position = transform.position;
                IsSpawned = true;
            }
        }
    }
}