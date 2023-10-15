using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ShardDropper : MonoBehaviour
    {
        [SerializeField]
        private int ShardAmount;

        [SerializeField]
        private float BaseDelay;

        private float Timer;
        private int ShardsLeft;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void OnEnable()
        {
            Timer = BaseDelay;
            ShardsLeft = ShardAmount;
        }

        private void FixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;
            //if (Timer > 0 || ShardsLeft <= 0) return;
            
            if (Timer <= 0 && ShardsLeft > 0)
            {
                Timer = BaseDelay;
                ShardsLeft--;

                var instance = ResourceManager.GetFromPool(GFXs.FallingShard);
                instance.transform.SetParent(DynamicsContainer.Transform, false);
                DynamicsContainer.AddItem(instance);

                var newX = transform.position.x + Random.Range(-4f, 4f);
                var newPosition = new Vector2(newX, transform.position.y);
                var shard = instance.GetComponent<FallingShard>();
                shard.Initiate(newPosition, Random.Range(-1f, 1f));
            }
        }

        public void Initiate(Vector2 newPosition)
        {
            transform.position = newPosition;
        }
    }
}