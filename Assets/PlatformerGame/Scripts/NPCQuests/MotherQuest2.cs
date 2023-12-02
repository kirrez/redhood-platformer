using UnityEngine.UI;
using UnityEngine;

// changes MotherPie from 2 to 3 if proper amount of mushrooms and berries collected
// changes MotherPie from 3 to 4

namespace Platformer
{
    public class MotherQuest2 : MonoBehaviour
    {
        [SerializeField]
        MotherPieSpawner Spawner;

        [SerializeField]
        Text HelpText;

        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;
        private int DialoguePhase = 0;

        private int Berries;
        private int Schrooms;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            AreaTrigger = GetComponent<Collider2D>();

            HelpText.text = Localization.Text(ETexts.TalkToMomAgain);
            HelpText.gameObject.SetActive(false);
        }

        private void Update()
        {
            Berries = ProgressManager.GetQuest(EQuest.BlackberriesCollected);
            Schrooms = ProgressManager.GetQuest(EQuest.MushroomsCollected);

            if (Berries >= ProgressManager.GetQuest(EQuest.BlackberriesRequired) && Schrooms >= ProgressManager.GetQuest(EQuest.MushroomsRequired) && ProgressManager.GetQuest(EQuest.MotherPie) < 3)
            {
                ProgressManager.SetQuest(EQuest.MotherPie, 3);
            }

            if (ProgressManager.GetQuest(EQuest.MotherPie) != 3) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                HelpText.gameObject.SetActive(true);
                Player.Interaction += OnInteraction;
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue2));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie2_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_6));
                    DialoguePhase++;
                    break;
                case 6:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;

                    ProgressManager.SetQuest(EQuest.MotherPie, 4);
                    Spawner.SpawnItem();
                    break;
            }
        }
    }
}