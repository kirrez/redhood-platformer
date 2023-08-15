using UnityEngine;

namespace Platformer
{
    public class PlatformActivation : MonoBehaviour
    {
        // base - WFFirstElevator
        [SerializeField]
        [Range(0, 5)]
        private int ItemIndex;

        [SerializeField]
        private GameObject Dummy;

        [SerializeField]
        private GameObject Platform;

        private IProgressManager ProgressManager;

        private int TargetValue;
        private EQuest Item;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();

            TargetValue = (int)EQuest.WFFirstElevator + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnEnable()
        {
            if (ProgressManager.GetQuest(Item) == 1)
            {
                Dummy.SetActive(false);
                Platform.SetActive(true);
            }

            if (ProgressManager.GetQuest(Item) == 0)
            {
                Dummy.SetActive(true);
                Platform.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(Item) == 0 && collision.gameObject.CompareTag("Player"))
            {
                ProgressManager.SetQuest(Item, 1);
                Dummy.SetActive(false);
                Platform.SetActive(true);
            }
        }
    }
}