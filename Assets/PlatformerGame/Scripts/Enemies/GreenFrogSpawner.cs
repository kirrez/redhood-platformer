using UnityEngine;

namespace Platformer
{
    public class GreenFrogSpawner : MonoBehaviour
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
        private IPlayer Player;
        private GreenFrog Frog;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Collider = GetComponent<Collider2D>();
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

        private void FixedUpdate()
        {
            float direction;

            if (Timer > 0 && !FrogSpawned)
            {
                Timer -= Time.fixedDeltaTime;
            }

            if (Timer <= 0 && isActive && !FrogSpawned)
            {
                FrogSpawned = true;

                StartX = Player.Position.x;
                // frog jumps from direction, where player is facing
                direction = Player.DirectionCheck() * -1;
                StartX += InitialDistance * direction * -1;
                StartY = transform.position.y - Collider.bounds.extents.y;

                var instance = ResourceManager.GetFromPool(Enemies.GreenFrog);
                var dynamics = CompositionRoot.GetDynamicsContainer();
                instance.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(instance);
                Frog = instance.GetComponent<GreenFrog>();

                Frog.Initiate(direction, StartY, new Vector2(StartX, StartY));
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