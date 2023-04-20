using UnityEngine;

namespace Platformer
{
    public class WaterSplasher : MonoBehaviour
    {
        private IResourceManager ResourceManager;
        private Collider2D Collider;
        private IPlayer Player;
        private bool Inside;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (Player == null) return;

            if (Collider.bounds.Contains(Player.Position) && !Inside)
            {
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                effect.transform.position = Player.Position;
                Inside = true;
            }

            if (!Collider.bounds.Contains(Player.Position) && Inside)
            {
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                effect.transform.position = Player.Position;
                Inside = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
            }
        }
    }
}