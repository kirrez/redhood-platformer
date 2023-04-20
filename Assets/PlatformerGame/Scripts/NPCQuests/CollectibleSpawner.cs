using UnityEngine;

namespace Platformer
{
    public class CollectibleSpawner : MonoBehaviour
    {
        private IProgressManager ProgressManager;

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
            var instance = GameObject.Instantiate(ItemPrefab);
            instance.transform.position = transform.position;

            if (ProgressManager.GetQuest(Item) != 0)
            {
                instance.SetActive(false);
            }
        }
    }
}