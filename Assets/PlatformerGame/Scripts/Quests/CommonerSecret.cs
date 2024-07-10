using UnityEngine;

namespace Platformer
{
    public class CommonerSecret : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                ShowMessage(Localization.Label(ELabels.Talk));
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
            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Commoner_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.CommonerSecret));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= OnInteraction;
                    Inside = false;
                    break;
            }
        }
    }
}