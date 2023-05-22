using UnityEngine;

namespace Platformer
{
    public class WaterSplasher : MonoBehaviour
    {
        private IResourceManager ResourceManager;
        private IDynamicsContainer Dynamics;
        private Collider2D Collider;
        private IPlayer Player;
        private bool Inside;

        private void Start()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Dynamics = CompositionRoot.GetDynamicsContainer();
            Collider = GetComponent<Collider2D>();
            Player = CompositionRoot.GetPlayer();
        }

        private void Update()
        {

            if (Collider.bounds.Contains(Player.Position) && !Inside)
            {
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                effect.transform.SetParent(Dynamics.Transform, false);
                Dynamics.AddItem(effect);

                effect.transform.position = Player.Position;
                Inside = true;
            }

            if (!Collider.bounds.Contains(Player.Position) && Inside)
            {
                var effect = ResourceManager.GetFromPool(GFXs.BlueSplash);
                effect.transform.SetParent(Dynamics.Transform, false);
                Dynamics.AddItem(effect);

                effect.transform.position = Player.Position;
                Inside = false;
            }
        }

    }
}