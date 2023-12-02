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
        private int DialoguePhase;

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
            int quest = ProgressManager.GetQuest(EQuest.MotherPie);
            //if (quest != 0) return;

            if (quest == 0)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    DialoguePhase = 0;
                    Player.Interaction += MQ1First;
                    HelpText.gameObject.SetActive(true);
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= MQ1First;
                    HelpText.gameObject.SetActive(false);
                }
            }
            
            if (quest > 0 && quest < 3)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    DialoguePhase = 0;
                    Player.Interaction += MQ1Second;
                    HelpText.gameObject.SetActive(true);
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= MQ1Second;
                    HelpText.gameObject.SetActive(false);
                }
            }
        }

        private void MQ1First()
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

                    ProgressManager.SetQuest(EQuest.KeyRed, 0);
                    Spawner.SpawnItem();
                    Player.Interaction -= MQ1First;
                    break;
            }
        }

        private void MQ1Second()
        {
            var Game = CompositionRoot.GetGame();

            var schrooms = ProgressManager.GetQuest(EQuest.MushroomsCollected);
            var berries = ProgressManager.GetQuest(EQuest.BlackberriesCollected);
            string phrase = Localization.Text(ETexts.DialoguePie1_7) + schrooms.ToString() + Localization.Text(ETexts.DialoguePie1_8) + berries.ToString() + Localization.Text(ETexts.DialoguePie1_9);

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue1));
                    Game.Dialogue.ChangeContent(phrase);
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= MQ1Second;
                    break;
            }

        }

    }
}