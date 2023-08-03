using UnityEngine;

namespace Platformer
{
    public class Blackberry : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 3)]
        private int ItemIndex;

        private int TargetValue;
        private EQuest Item;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();

            TargetValue = (int)EQuest.Blackberry0 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    //increment blackberry current
                    ProgressManager.AddValue(EQuest.BlackberriesCollected, 1);
                }
                gameObject.SetActive(false);
            }
        }
    }
}