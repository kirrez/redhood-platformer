using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BatSpawner : MonoBehaviour
    {
        [SerializeField]
        private bool SpawnOnOneSide; // false - both sides, true - one side;

        [SerializeField]
        private bool CurrentDirection; // false : Left to Right, true - vice versa

        [SerializeField]
        private float RespawnCooldown = 2f;

        [SerializeField]
        private float BatSpeed = 300f;

        [SerializeField]
        private float SpawnDistance = 15f;

        private IResourceManager ResourceManager;

        private float Timer = 0f;
        private bool isActive;
        private bool BatSpawned;
        private float StartY;
        private float StartX;
        private IPlayer Player;
        private Bat Bat;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Player = CompositionRoot.GetPlayer();
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

        private void Update()
        {
            float direction;
            Timer -= Time.deltaTime;

            if (isActive && !BatSpawned && Timer <= 0f)
            {
                //Debug.Log("Bat Spawned!");
                BatSpawned = true;

                StartY = Player.Position.y + 2f;
                StartX = Player.Position.x;

                if (!SpawnOnOneSide)
                {
                    CurrentDirection = !CurrentDirection;
                }

                if (!CurrentDirection)
                {
                    StartX -= SpawnDistance; // magic number
                    direction = 1;
                }
                else
                {
                    StartX += SpawnDistance;
                    direction = -1f;
                }

                var instance = ResourceManager.GetFromPool(Enemies.Bat);
                instance.transform.SetParent(transform.parent, false);
                Bat = instance.GetComponent<Bat>();
                Bat.Initiate(direction, new Vector2(StartX, StartY), BatSpeed);
                // keep only one active "OnBatKilled"
                Bat.Killed -= OnBatKilled;
                Bat.Killed += OnBatKilled;
            }

        }

        public void OnBatKilled()
        {
            BatSpawned = false;
            Timer = RespawnCooldown;
        }
    }
}