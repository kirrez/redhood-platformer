using UnityEngine;

namespace Platformer
{
    public class SingleActivation : MonoBehaviour
    {
        [SerializeField]
        private int ItemIndex; // instead of enum variable

        [SerializeField]
        private int ConditionalValue;

        [SerializeField]
        private int TargetValue;


        [SerializeField]
        private GameObject Dummy;

        [SerializeField]
        private GameObject Elevator;

        private IProgressManager ProgressManager;
        private EQuest Quest;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Quest = (EQuest)ItemIndex;
        }


        private void OnEnable()
        {
            if (ProgressManager.GetQuest(Quest) == TargetValue)
            {
                Dummy.SetActive(false);
                Elevator.SetActive(true);
            }

            if (ProgressManager.GetQuest(Quest) != TargetValue)
            {
                Dummy.SetActive(true);
                Elevator.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(Quest) == ConditionalValue && collision.gameObject.CompareTag("Player"))
            {
                ProgressManager.SetQuest(Quest, TargetValue);
                Dummy.SetActive(false);
                Elevator.SetActive(true);
            }
        }
    }
}