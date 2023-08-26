using UnityEngine.UI;
using UnityEngine;

// Changes Mother's pie value from 0 to 1, spawns Red key in cellar

namespace Platformer
{
    public class MotherQuest1 : MonoBehaviour
    {
        [SerializeField]
        private KeySpawner Spawner;

        [SerializeField]
        Text HelpText;

        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;
        private int DialoguePhase = 0;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();
            
            HelpText.text = Localization.Text(ETexts.TalkToMom);
            HelpText.gameObject.SetActive(false);

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 0) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                HelpText.gameObject.SetActive(true);
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HelpText.gameObject.SetActive(false);
            }
        }

        private void OnInteraction()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue1));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie1_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_6));
                    DialoguePhase++;
                    break;
                case 6:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.MotherPie, 1);
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;

                    ProgressManager.SetQuest(EQuest.KeyRed, 0);
                    Spawner.SpawnItem();
                    break;
            }

        }
    }
}