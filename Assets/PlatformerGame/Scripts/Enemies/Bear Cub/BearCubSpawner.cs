using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BearCubSpawner : MonoBehaviour
    {
        [SerializeField]
        private Collider2D TriggerZone;

        [SerializeField]
        private Transform SpawnPoint;

        [SerializeField]
        private float RespawnCooldown;

        [SerializeField]
        private bool AlwaysSpawn;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IPlayer Player;

        private float Timer = 0f;
        private bool InsideTrigger;
        private bool IsSpawned;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            if (AlwaysSpawn == true)
            {
                InsideTrigger = true;
            }
        }

        private void FixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;

            if (AlwaysSpawn == false)
            {
                if (TriggerZone.bounds.Contains(Player.Position))
                {
                    InsideTrigger = true;
                }
                else
                {
                    InsideTrigger = false;
                }
            }
            

            if (Timer <= 0 && InsideTrigger == true && IsSpawned == false)
            {
                Timer = RespawnCooldown;

                var instance = ResourceManager.GetFromPool(Enemies.BearCub);
                DynamicsContainer.AddEnemy(instance);
                BearCub BearCub = instance.GetComponent<BearCub>();
                BearCub.Initiate(SpawnPoint.position);

                IsSpawned = true;
                BearCub.Killed -= OnBearKilled;
                BearCub.Killed += OnBearKilled;
            }
        }

        public void OnBearKilled()
        {
            Timer = RespawnCooldown;
            IsSpawned = false;
        }
    }
}