using UnityEngine;

// changes MotherPie from 2 to 3 if proper amount of mushrooms and berries collected
// changes MotherPie from 3 to 4

namespace Platformer
{
    public class MotherQuest2 : BaseQuest
    {
        [SerializeField]
        MotherPieSpawner Spawner;

        private int Berries;
        private int Schrooms;

        protected override void RequirementsCheck()
        {
            Berries = ProgressManager.GetQuest(EQuest.BlackberriesCollected);
            Schrooms = ProgressManager.GetQuest(EQuest.MushroomsCollected);

            if (Berries >= ProgressManager.GetQuest(EQuest.BlackberriesRequired) && Schrooms >= ProgressManager.GetQuest(EQuest.MushroomsRequired) && ProgressManager.GetQuest(EQuest.MotherPie) < 3)
            {
                ProgressManager.SetQuest(EQuest.MotherPie, 3);
            }

            if (ProgressManager.GetQuest(EQuest.MotherPie) != 3) return;

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                ShowMessage(Localization.Label(ELabels.TalkToMom));
                Player.Interaction += OnInteraction;
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HideMessage();
            }
        }

        private void OnInteraction()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue2_Title));
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
                    HideMessage();
                    Player.Interaction -= OnInteraction;

                    ProgressManager.SetQuest(EQuest.MotherPie, 4);
                    Spawner.SpawnItem();
                    Inside = false;
                    break;
            }
        }
    }
}