using UnityEngine;

namespace Platformer
{
    public class Mushroom : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 2)]
        private int ItemIndex;

        private int TargetValue;
        private EQuest Item;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();

            TargetValue = (int)EQuest.Mushroom0 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    //increment MushroomsCollected
                    ProgressManager.AddValue(EQuest.MushroomsCollected, 1);
                }
                gameObject.SetActive(false);
            }
        }
    }
}