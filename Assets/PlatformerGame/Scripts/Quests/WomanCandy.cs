using UnityEngine;

namespace Platformer
{
    public class WomanCandy : BaseQuest
    {
        [SerializeField]
        private FoodSpawner Spawner;

        [SerializeField]
        private NPCWanderer Wanderer;

        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.WomanCandy) == 1) return;

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
            }
        }

        private void OnInteraction()
        {
            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Commoner_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.WomanCandy));
                    ProgressManager.SetQuest(EQuest.WomanCandy, 1);
                    Wanderer.Stun(true);
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    Player.Interaction -= OnInteraction;
                    Spawner.gameObject.SetActive(true);
                    Wanderer.Stun(false);
                    Inside = false;
                    break;
            }
        }
    }
}