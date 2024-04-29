using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class KitchenKnifeItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 JumpForce;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;

        private bool IsSpawned;

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
            if (ProgressManager.GetQuest(EQuest.KitchenKnifeItem) == 1 && !IsSpawned)
            {
                var instance = ResourceManager.CreatePrefab<KitchenKnifeItem, EQuest>(EQuest.KitchenKnifeItem);
                instance.PhysicsOn(true);

                instance.transform.SetParent(gameObject.transform, false);
                instance.GetComponent<Rigidbody2D>().AddForce(JumpForce, ForceMode2D.Impulse);

                IsSpawned = true;
            }
        }
    }
}