using UnityEngine;

namespace Platformer
{
    public class GatekeeperTip : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            int quest = ProgressManager.GetQuest(EQuest.KeyGrey);
            //if ( quest == 1) return;

            if (quest < 1)
            {
                if (Trigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += Gatekeeper1;
                    ShowMessage(Localization.Text(ETexts.Talk));
                }

                if (!Trigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Gatekeeper1;
                    HideMessage();
                }
            }

            if (quest >= 1)
            {
                if (Trigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += Gatekeeper2;
                    ShowMessage(Localization.Text(ETexts.Talk));
                }

                if (!Trigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Gatekeeper2;
                    HideMessage();
                }
            }
        }

        private void Gatekeeper1()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Gatekeeper1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= Gatekeeper1;
                    break;
            }
        }

        private void Gatekeeper2()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Gatekeeper1_2));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= Gatekeeper2;
                    break;
            }
        }
    }
}