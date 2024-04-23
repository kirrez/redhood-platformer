using UnityEngine;

namespace Platformer
{
    public class MarketElevatorTip : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.MarketElevator) > 1) return; // Elevator works when value becomes "2" 

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                ShowMessage(Localization.Text(ETexts.Talk));
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.VillageCommoner));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Commoner1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HideMessage();
                    Player.Interaction -= OnInteraction;
                    DialoguePhase = 0;
                    break;
            }

        }
    }
}