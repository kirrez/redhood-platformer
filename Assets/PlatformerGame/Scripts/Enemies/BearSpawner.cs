using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BearSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject SpawnPoint;

        [SerializeField]
        private Collider2D DeathTrigger;

        [SerializeField]
        private float RespawnCooldown = 3f;
        [SerializeField]
        private int MaxSpawnCount = 2;

        private IResourceManager ResourceManager;

        private float Timer = 0f;
        private bool isActive;
        private int SpawnCount;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isActive = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isActive = false;
            }
        }

        private void FixedUpdate()
        {
            float direction = 1f;
            float chance;
            Bear Bear;

            if (Timer > 0 && Timer <= RespawnCooldown && !isActive && SpawnCount < MaxSpawnCount)
            {
                Timer += Time.fixedDeltaTime;
            }

            if (Timer > 0 && isActive && SpawnCount < MaxSpawnCount)
            {
                Timer -= Time.fixedDeltaTime;
            }

            if (Timer <=0 && isActive && SpawnCount < MaxSpawnCount)
            {
                Timer = RespawnCooldown;
                SpawnCount++;

                chance = Random.Range(0f, 1f);
                if (chance < 0.5f)
                {
                    direction = -1f;
                }
                if (chance >= 0.5f)
                {
                    direction = 1f;
                }

                var instance = ResourceManager.GetFromPool(Enemies.Bear);
                var dynamics = CompositionRoot.GetDynamicsContainer();
                instance.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(instance);
                Bear = instance.GetComponent<Bear>();

                Bear.Initiate(direction, SpawnPoint.transform.position, DeathTrigger);
                Bear.Killed -= OnBearKilled;
                Bear.Killed += OnBearKilled;
            }
        }

        public void OnBearKilled()
        {
            SpawnCount--;
            if (Timer <= 0)
            {
                Timer = RespawnCooldown;
            }
        }
    }
}