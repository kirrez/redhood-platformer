using UnityEngine.UI;
using Platformer.UI;
using UnityEngine;

namespace Platformer
{
    public class MotherQuest1 : MonoBehaviour
    {
        [SerializeField]
        Transform KeyPosition;

        [SerializeField]
        Text HelpText;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private int DialoguePhase = 0;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
        }

        private void OnEnable()
        {
            HelpText.text = Localization.Text(ETexts.TalkToMom);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 0) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                Player.Interaction += OnQuestCompleted;
                HelpText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 0) return;

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
            var Game = CompositionRoot.GetGame();

            if (DialoguePhase == 0)
            {
                Game.Dialogue.Show();
                Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue1));
                Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie1_1));

                DialoguePhase++;
                return;
            }

            if (DialoguePhase == 1)
            {
                //Debug.Log("DPhase = 1");
                Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_2));

                DialoguePhase++;
                return;
            }

            if (DialoguePhase == 2)
            {
                Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_3));

                DialoguePhase++;
                return;
            }

            if (DialoguePhase == 3)
            {
                Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_4));

                DialoguePhase++;
                return;
            }

            if (DialoguePhase == 4)
            {
                //Debug.Log("DPhase = 4");
                Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie1_5));

                DialoguePhase++;
                return;
            }

            if (DialoguePhase == 5)
            {
                Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_6));

                DialoguePhase++;
                return;
            }

            if (DialoguePhase == 6)
            {
                Game.Dialogue.Hide();
                ProgressManager.SetQuest(EQuest.MotherPie, 1);
                HelpText.gameObject.SetActive(false);
                Player.Interaction -= OnQuestCompleted;

                var instance = ResourceManager.CreatePrefab<KeyRed, ECollectibles>(ECollectibles.KeyRed);
                instance.transform.SetParent(transform, false);
                instance.transform.position = KeyPosition.position;
            }
        }
    }
}