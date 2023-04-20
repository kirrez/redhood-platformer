using UnityEngine;

namespace Platformer
{
    public class Blackberry : MonoBehaviour
    {
        [SerializeField]
        private EQuest Item;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    ProgressManager.AddValue(EQuest.BerriesCurrent, 1);
                }
                gameObject.SetActive(false);
            }
        }
    }
}