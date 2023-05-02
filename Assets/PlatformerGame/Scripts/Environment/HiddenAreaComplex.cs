using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Platformer
{
    public class HiddenAreaComplex : MonoBehaviour
    {
        [SerializeField]
        private TilemapRenderer Area;

        public List<Collider2D> Entrances;

        public List<Collider2D> Exits;

        private IPlayer Player;
        private bool Inside;

        private void Start()
        {
            Player = CompositionRoot.GetPlayer();
        }

        private void Update()
        {
            if (Player == null) return;

            foreach (Collider2D entrance in Entrances)
            {
                if (entrance.bounds.Contains(Player.Position) && !Inside)
                {
                    Area.enabled = false;
                    Inside = true;
                }
            }

            foreach (Collider2D exit in Exits)
            {
                if (exit.bounds.Contains(Player.Position) && Inside)
                {
                    Area.enabled = true;
                    Inside = false;
                }
            }
        }
    }
}