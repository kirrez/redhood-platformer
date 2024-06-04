using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ZombiePit : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.ZombiePit);

            if (quest == 0)
            {
                if (Trigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += OnInteraction;
                    OnInteraction();
                }

                if (!Trigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= OnInteraction;
                }
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.ZombiePit_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.ZombiePit_1));

                    AudioManager.PlaySound(ESounds.Zombie_2);
                    DialoguePhase++;
                    break;

                case 1:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.ZombiePit_2));
                    DialoguePhase++;
                    break;

                case 2:
                    Player.ReleasedByInteraction();
                    ProgressManager.SetQuest(EQuest.ZombiePit, 1);
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    Player.Interaction -= OnInteraction;
                    Inside = false;
                    break;
            }
        }
    }
}