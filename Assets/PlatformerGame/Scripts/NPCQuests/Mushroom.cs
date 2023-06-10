using UnityEngine;

namespace Platformer
{
    public class Mushroom : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 3)]
        private int ItemIndex = 1;

        private EQuest Item;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            DefineItem();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    ProgressManager.AddValue(EQuest.MushroomsCurrent, 1);
                }
                gameObject.SetActive(false);
            }
        }

        private void DefineItem()
        {
            switch (ItemIndex)
            {
                case 1:
                    Item = EQuest.Mushroom1;
                    break;
                case 2:
                    Item = EQuest.Mushroom2;
                    break;
                case 3:
                    Item = EQuest.Mushroom3;
                    break;
            }
        }
    }
}