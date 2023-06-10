using UnityEngine;

namespace Platformer
{
    public class Blackberry : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 4)]
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
                    ProgressManager.AddValue(EQuest.BerriesCurrent, 1);
                }
                gameObject.SetActive(false);
            }
        }

        private void DefineItem()
        {
            switch (ItemIndex)
            {
                case 1:
                    Item = EQuest.Blackberry1;
                    break;
                case 2:
                    Item = EQuest.Blackberry2;
                    break;
                case 3:
                    Item = EQuest.Blackberry3;
                    break;
                case 4:
                    Item = EQuest.Blackberry4;
                    break;
            }
        }
    }
}