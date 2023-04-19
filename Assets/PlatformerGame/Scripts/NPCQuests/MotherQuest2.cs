using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class MotherQuest2 : MonoBehaviour
    {
        [SerializeField]
        Transform PiePosition;

        [SerializeField]
        ETexts Label;

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

        private void Start()
        {
            HelpText.text = Localization.Text(Label);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 1) return;

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
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 1) return;

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
            ProgressManager.SetQuest(EQuest.MotherPie, 2);
            HelpText.gameObject.SetActive(false);
            Player.Interaction -= OnQuestCompleted;
            
        }
    }
}