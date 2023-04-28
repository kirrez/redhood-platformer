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

        private void Update()
        {
            if (Player == null) return;


        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                Area.enabled = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Area.enabled = true;
            }
        }
    }
}