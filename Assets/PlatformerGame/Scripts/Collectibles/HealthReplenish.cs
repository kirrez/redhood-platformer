using UnityEngine;

namespace Platformer
{
    public class HealthReplenish : MonoBehaviour
    {
        private IPlayer Player;

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.UpdateMaxLives();
                gameObject.SetActive(false);
            }
        }
    }
}