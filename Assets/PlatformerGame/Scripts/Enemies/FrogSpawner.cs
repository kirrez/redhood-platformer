using UnityEngine;

namespace Platformer
{
    public class FrogSpawner : MonoBehaviour
    {
        [SerializeField]
        private float RespawnCooldown = 3f;
        [SerializeField]
        private float InitialDistance = 7f;

        private IResourceManager ResourceManager;

        private float Timer = 0f;
        private bool isActive;
        private bool FrogSpawned;
        private float StartY;
        private float StartX;
        private Collider2D Collider; // for calculating boundaries of spawner
        private GameObject Player;
        private Frog Frog;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isActive = true;
                Player = collision.gameObject;
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
            float direction;
            Timer -= Time.fixedDeltaTime;

            if (isActive && !FrogSpawned && Timer <= 0)
            {
                FrogSpawned = true;

                StartX = Player.transform.position.x;
                // frog jumps from direction, where player is facing
                direction = Player.GetComponent<Player>().DirectionCheck() * -1;
                StartX += InitialDistance * direction * -1;
                StartY = transform.position.y - Collider.bounds.extents.y;

                var instance = ResourceManager.GetFromPool(Enemies.Frog);
                Frog = instance.GetComponent<Frog>();

                Frog.Initiate(direction, StartY, new Vector2(StartX, StartY), Player.GetComponent<Player>());
                Frog.Killed -= OnFrogKilled;
                Frog.Killed += OnFrogKilled;

                //BlueSplash effect
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                effect.transform.position = new Vector2(StartX, StartY);
            }
        }

        public void OnFrogKilled()
        {
            FrogSpawned = false;
            Timer = RespawnCooldown;
        }
    }
}