using UnityEngine;
using UnityEngine.UI;

// changes Mother's pie from 3 to 4

namespace Platformer
{
    public class MotherQuest2 : MonoBehaviour
    {
        [SerializeField]
        Transform PiePosition;

        [SerializeField]
        Text HelpText;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
        }

        private void OnEnable()
        {
            HelpText.text = Localization.Text(ETexts.TalkToMomAgain);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 3) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                HelpText.gameObject.SetActive(true);

                var BerriesEnough = ProgressManager.GetQuest(EQuest.BerriesCurrent) >= ProgressManager.GetQuest(EQuest.BerriesRequired);
                var MushroomsEnough = ProgressManager.GetQuest(EQuest.MushroomsCurrent) >= ProgressManager.GetQuest(EQuest.MushroomsRequired);
                
                if (BerriesEnough && MushroomsEnough)
                {
                    Player.Interaction += OnQuestCompleted;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 3) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                if (Player != null)
                {
                    Player.Interaction -= OnQuestCompleted;
                }
                HelpText.gameObject.SetActive(false);
            }
        }

        private void OnQuestCompleted()
        {
            // should be a dialogue and spawn of pie. Also Spawned pie will unlock life upgrade ))
            // also market elevator becomes enabled from this point
            ProgressManager.SetQuest(EQuest.MotherPie, 4);
            HelpText.gameObject.SetActive(false);
            Player.Interaction -= OnQuestCompleted;
            
        }
    }
}