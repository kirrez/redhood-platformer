using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TentacleShardDropper : MonoBehaviour
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

            if (Timer <= 0 && ShardsLeft > 0)
            {
                Timer = BaseDelay;
                ShardsLeft--;

                var instance = ResourceManager.GetFromPool(GFXs.GreenShard);
                DynamicsContainer.AddMain(instance);

                var newX = transform.position.x + Random.Range(-0.7f, 0.7f);
                var newPosition = new Vector2(newX, transform.position.y);
                var shard = instance.GetComponent<FallingShard>();
                shard.Initiate(newPosition, Random.Range(-1f, 1f));
            }
        }
    }
}