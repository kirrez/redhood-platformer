using UnityEngine;

namespace Platformer
{
    public class FoundLeverHandle : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.BrokenLeverTip);

            if (quest > 0) return;

            if (Trigger.bounds.Contains(Player.Position) && Inside == false)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                OnInteraction();
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.BrokenLeverTip_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.BrokenLever1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.BrokenLeverTip, 1);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}