using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class WestForestDrawbridge : MonoBehaviour
    {
        [SerializeField]
        private ETexts Label;

        [SerializeField]
        private Text HelpText;

        [SerializeField]
        private GameObject Drawbridge;

        [SerializeField]
        private Sprite SwitchOff;

        [SerializeField]
        private Sprite SwitchOn;

        private IProgressManager ProgressManager;
        private ILocalization Localization;

        private IPlayer Player;
        private SpriteRenderer Renderer;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            HelpText.text = Localization.Text(Label);

            if (ProgressManager.GetQuest(EQuest.Drawbridge) == 0)
            {
                Renderer.sprite = SwitchOff;
                Drawbridge.SetActive(false);
            }

            if (ProgressManager.GetQuest(EQuest.Drawbridge) !=0)
            {
                Renderer.sprite = SwitchOn;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.Drawbridge) != 0) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                Player.Interaction += OnQuestCompleted;
                HelpText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.Drawbridge) != 0) return;

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
            ProgressManager.SetQuest(EQuest.Drawbridge, 1);
            HelpText.gameObject.SetActive(false);
            Player.Interaction -= OnQuestCompleted;

            Drawbridge.SetActive(true);
            Renderer.sprite = SwitchOn;
        }
    }
}