using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class KillerfishSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform Spawnpoint;

        [SerializeField]
        private List<Transform> Waypoints;

        private float RespawnCooldown = 6f;

        private IResourceManager ResourceManager;

        private bool FishSpawned;
        private float Timer = 0f;
        private Killerfish Killerfish;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        private void Update()
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
            }

            if (!FishSpawned && Timer <= 0)
            {
                FishSpawned = true;
                var instance = ResourceManager.GetFromPool(Enemies.Killerfish);
                var dynamics = CompositionRoot.GetDynamicsContainer();
                instance.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(instance);
                Killerfish = instance.GetComponent<Killerfish>();
                Killerfish.Initiate(Spawnpoint.position, Waypoints);
                // keep only one active "OnFishKilled"
                Killerfish.Killed -= OnFishKilled;
                Killerfish.Killed += OnFishKilled;
            }
        }

        public void OnFishKilled()
        {
            FishSpawned = false;
            Timer = RespawnCooldown;
        }
    }
}