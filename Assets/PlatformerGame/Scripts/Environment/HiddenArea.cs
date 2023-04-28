using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformer
{
    public class HiddenArea : MonoBehaviour
    {
        [SerializeField]
        private TilemapRenderer Area;

        private bool Inside;
        private IPlayer Player;

        private Collider2D Collider;

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (Player == null) return;

            if (Collider.bounds.Contains(Player.Position) && !Inside)
            {
                Area.enabled = false;
                Inside = true;
            }

            if (!Collider.bounds.Contains(Player.Position) && Inside)
            {
                Area.enabled = true;
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