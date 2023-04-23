using UnityEngine;

namespace Platformer
{
    public class SingleActivation : MonoBehaviour
    {
        [SerializeField]
        private EQuest Quest;

        [SerializeField]
        private GameObject Dummy;

        [SerializeField]
        private GameObject Elevator;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnEnable()
        {
            if (ProgressManager.GetQuest(Quest) == 0)
            {
                Dummy.SetActive(true);
                Elevator.SetActive(false);
            }

            if (ProgressManager.GetQuest(Quest) !=0)
            {
                Dummy.SetActive(false);
                Elevator.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(Quest) == 0 && collision.gameObject.CompareTag("Player"))
            {
                ProgressManager.SetQuest(Quest, 1);
                Dummy.SetActive(false);
                Elevator.SetActive(true);
            }
        }
    }
}