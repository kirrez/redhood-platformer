using UnityEngine;

namespace Platformer
{
    public class CollectibleSpawner2Conditions : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private bool IsSpawned;

        //It's not very safe, but a mistake would be quiet obvious
        [SerializeField]
        private EQuest Quest1;

        [SerializeField]
        private int ConditionValue1 = 0;

        [SerializeField]
        private EQuest Quest2;

        [SerializeField]
        private int ConditionValue2 = 0;

        [SerializeField]
        private GameObject ItemPrefab;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnEnable()
        {
            if (IsSpawned) return;

            if (ProgressManager.GetQuest(Quest1) == ConditionValue1 && ProgressManager.GetQuest(Quest2) == ConditionValue2)
            {
                SpawnItem();
            }
        }

        public void SpawnItem()
        {
            var instance = GameObject.Instantiate(ItemPrefab, transform.parent, false);
            instance.transform.SetParent(gameObject.transform, false);
            IsSpawned = true;
        }
    }
}