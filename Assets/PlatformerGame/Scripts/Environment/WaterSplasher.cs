using UnityEngine;

namespace Platformer
{
    public class WaterSplasher : MonoBehaviour
    {
        private IResourceManager ResourceManager;
        private Collider2D Collider;
        private IPlayer Player;
        private bool Inside;

        private void Start()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Collider = GetComponent<Collider2D>();
            Player = CompositionRoot.GetPlayer();
        }

        private void Update()
        {

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

    }
}