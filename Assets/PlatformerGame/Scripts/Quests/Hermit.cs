using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Hermit : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            int quest = ProgressManager.GetQuest(EQuest.Hermit);

            if (quest == 0)
            {
                if (Trigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += Hermit1;
                    ShowMessage(Localization.Label(ELabels.Talk));
                }

                if (!Trigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Hermit1;
                    HideMessage();
                }
            }

            if (quest == 1)
            {
                if (Trigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += Hermit2;
                    ShowMessage(Localization.Label(ELabels.Talk));
                }

                if (!Trigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Hermit2;
                    HideMessage();
                }
            }
        }

        private void Hermit1()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Hermit_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Hermit1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Hermit1_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Hermit1_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Hermit1_4));
                    DialoguePhase++;
                    break;
                case 4:
                    ProgressManager.SetQuest(EQuest.Hermit, 1);

                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= Hermit1;
                    Inside = false;
                    break;
            }
        }

        private void Hermit2()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Hermit_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Hermit2));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= Hermit2;
                    Inside = false;
                    break;
            }
        }
    }
}