using UnityEngine;

namespace Platformer
{
    public class MarketElevatorTip : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.MarketElevator);

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;

                if (quest == 0) Player.Interaction += OnInteraction1;
                if (quest > 0) Player.Interaction += OnInteraction2;

                ShowMessage(Localization.Label(ELabels.Talk));
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;

                if (quest == 0) Player.Interaction -= OnInteraction1;
                if (quest > 0) Player.Interaction -= OnInteraction2;

                HideMessage();
            }
        }

        private void OnInteraction1()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Commoner_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.VillageElevator_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HideMessage();
                    Player.Interaction -= OnInteraction1;
                    DialoguePhase = 0;
                    Inside = false;
                    break;
            }
        }

        private void OnInteraction2()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Commoner_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.VillageElevator_2));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HideMessage();
                    Player.Interaction -= OnInteraction2;
                    DialoguePhase = 0;
                    Inside = false;
                    break;
            }
        }

    }
}