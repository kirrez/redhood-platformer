using UnityEngine;

namespace Platformer
{
    public class MotherPie : MonoBehaviour
    {
        private IProgressManager ProgressManager;

        private EQuest Item;

        private void OnEnable()
        {
            ProgressManager = CompositionRoot.GetProgressManager();

            Item = EQuest.MotherPie;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 4)
                {
                    ProgressManager.SetQuest(Item, 5);
                }

                gameObject.SetActive(false);
            }
        }
    }
}