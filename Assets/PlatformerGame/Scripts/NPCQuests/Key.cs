using UnityEngine;

namespace Platformer
{
    public class Key : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 3)]
        private int ItemIndex = 1;

        private EQuest Item;

        private IProgressManager ProgressManager;

        private void OnEnable()
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
                }

                gameObject.SetActive(false);
            }
        }

        private void DefineItem()
        {
            switch (ItemIndex)
            {
                case 1:
                    Item = EQuest.RedKey;
                    break;
                case 2:
                    Item = EQuest.GreyKey;
                    break;
                case 3:
                    Item = EQuest.GreenKey;
                    break;
            }
        }
    }
}