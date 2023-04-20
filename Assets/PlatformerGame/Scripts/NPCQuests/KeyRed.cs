using UnityEngine;

namespace Platformer
{
    public class KeyRed : MonoBehaviour
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
                if (ProgressManager.GetQuest(EQuest.RedKey) == 0)
                {
                    ProgressManager.SetQuest(EQuest.RedKey, 1);
                }

                gameObject.SetActive(false);
            }
        }
    }
}