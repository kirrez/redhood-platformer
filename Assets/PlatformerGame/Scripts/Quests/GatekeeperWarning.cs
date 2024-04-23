using UnityEngine;

namespace Platformer
{
    public class GatekeeperWarning : BaseQuest
    {
        protected override void RequirementsCheck()
        {
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Commoner1_2));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}