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

        private IPlayer Player;
        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private Collider2D Trigger;

        private float Timer = 0f;
        private bool Inside;
        private bool BatSpawned;
        private float StartY;
        private float StartX;
        private Bat Bat;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();

            Trigger = GetComponent<Collider2D>();
        }

        private void TriggerCheck()
        {
            if (Trigger.bounds.Contains(Player.Position) == true)
            {
                Inside = true;
            }

            if (Trigger.bounds.Contains(Player.Position) == false)
            {
                Inside = false;
            }
        }

        private void Spawn()
        {
            float direction;
            Timer -= Time.deltaTime;

            if (Inside && !BatSpawned && Timer <= 0f)
            {
                BatSpawned = true;

                StartY = Player.Position.y + 2f;
                StartX = Player.Position.x;

                if (!SpawnOnOneSide)
                {
                    CurrentDirection = !CurrentDirection;
                }

                if (!CurrentDirection)
                {
                    StartX -= SpawnDistance;
                    direction = 1;
                }
                else
                {
                    StartX += SpawnDistance;
                    direction = -1f;
                }

                var instance = ResourceManager.GetFromPool(Enemies.Bat);
                DynamicsContainer.AddEnemy(instance);
                Bat = instance.GetComponent<Bat>();
                Bat.Initiate(direction, new Vector2(StartX, StartY), BatSpeed);

                Bat.Killed -= OnBatKilled;
                Bat.Killed += OnBatKilled;

                AudioManager.PlaySound(ESounds.BatStart);
            }
        }

        private void Update()
        {
            TriggerCheck();
            Spawn();
        }

        public void OnBatKilled()
        {
            BatSpawned = false;
            Timer = RespawnCooldown;
        }
    }
}