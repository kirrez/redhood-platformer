using UnityEngine;

namespace Platformer
{
    public class MotherPie : MonoBehaviour
    {
        private IProgressManager ProgressManager;

        private void OnEnable()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(EQuest.MotherPie) == 4)
                {
                    ProgressManager.SetQuest(EQuest.MotherPie, 5); // Finishing MotherPie quest
                    ProgressManager.SetQuest(EQuest.MarketElevator, 1); // Unlocking elevator
                }

                gameObject.SetActive(false);
            }
        }
    }
}