using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HitEffectSpawner : MonoBehaviour
    {
        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var health = collision.gameObject.GetComponent<Health>();

            if (health != null && health.GetCharacterType() == 1)
            {
                if (health.DamageReceived() == false) return;

                Vector2 newPosition = collision.ClosestPoint(transform.position);

                var instance = ResourceManager.GetFromPool(GFXs.HitEffect);
                instance.transform.SetParent(DynamicsContainer.Transform, false);
                DynamicsContainer.AddItem(instance);

                instance.GetComponent<IBaseGFX>().Initiate(newPosition, 1f);
            }
        }

    }
}