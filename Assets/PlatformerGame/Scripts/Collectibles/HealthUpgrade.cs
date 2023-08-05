using UnityEngine;

namespace Platformer
{
    public class HealthUpgrade : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private IPlayer Player;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var maxLives = ProgressManager.GetQuest(EQuest.MaxLives);
                var maxLivesCap = ProgressManager.GetQuest(EQuest.MaxLivesCap);
                if (maxLives + 1 <= maxLivesCap)
                {
                    ProgressManager.AddValue(EQuest.MaxLives, 1);
                }

                Player.UpdateMaxLives();

                gameObject.SetActive(false);
            }
        }
    }
}