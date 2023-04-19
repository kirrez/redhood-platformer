using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformer
{
    public class HiddenArea : MonoBehaviour
    {
        [SerializeField]
        private TilemapRenderer Area;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
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