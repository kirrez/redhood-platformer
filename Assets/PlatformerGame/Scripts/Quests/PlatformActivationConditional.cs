using UnityEngine;

namespace Platformer
{
    // I need this script, when I want platform to become activated after some another quest variable being modified firstly
    public class PlatformActivationConditional : MonoBehaviour
    {
        // base - WFFirstElevator
        [SerializeField]
        [Range(0, 3)]
        private int ItemIndex;

        [SerializeField]
        private int InitialValue;

        [SerializeField]
        private int ModifiedValue;

        [SerializeField]
        private GameObject Dummy;

        [SerializeField]
        private GameObject Platform;

        [SerializeField]
        private bool EnableDiscoverySound;

        private IAudioManager AudioManager;
        private IProgressManager ProgressManager;

        private int TargetValue;
        private EQuest Item;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();

            TargetValue = (int)EQuest.WFFirstElevator + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnEnable()
        {
            if (ProgressManager.GetQuest(Item) == ModifiedValue)
            {
                Dummy.SetActive(false);
                Platform.SetActive(true);
            }

            if (ProgressManager.GetQuest(Item) != ModifiedValue)
            {
                Dummy.SetActive(true);
                Platform.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(Item) == InitialValue && collision.gameObject.CompareTag("Player"))
            {
                ProgressManager.SetQuest(Item, ModifiedValue);
                Dummy.SetActive(false);
                Platform.SetActive(true);

                if (EnableDiscoverySound)
                {
                    AudioManager.PlaySound(ESounds.ElevatorActivated);
                }
            }
        }
    }
}