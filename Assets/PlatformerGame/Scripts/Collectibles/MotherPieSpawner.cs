using UnityEngine;

namespace Platformer
{
    public class MotherPieSpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 JumpForce;

        private bool IsSpawned;
        private EQuest Item = EQuest.MotherPie;

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
            if (ProgressManager.GetQuest(EQuest.MotherPie) == 4 && !IsSpawned)
            {
                var instance = ResourceManager.CreatePrefab<MotherPie, EQuest>(Item);
                instance.PhysicsOn(true);
                instance.transform.SetParent(gameObject.transform, false);
                instance.GetComponent<Rigidbody2D>().AddForce(JumpForce, ForceMode2D.Impulse);

                IsSpawned = true;
            }
        }
    }
}