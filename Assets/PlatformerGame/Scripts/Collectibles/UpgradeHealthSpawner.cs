using UnityEngine;

namespace Platformer
{
    public class UpgradeHealthSpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 JumpForce;

        private EQuest Item = EQuest.UpgradeHealth;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;

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
            if (ProgressManager.GetQuest(Item) == 1)
            {
                var instance = ResourceManager.CreatePrefab<UpgradeHealth, EQuest>(Item);
                instance.PhysicsOn(true);

                instance.transform.SetParent(gameObject.transform, false);
                instance.GetComponent<Rigidbody2D>().AddForce(JumpForce, ForceMode2D.Impulse);
            }
        }
    }
}