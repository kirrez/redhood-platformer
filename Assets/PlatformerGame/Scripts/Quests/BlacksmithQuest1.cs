using UnityEngine;

// Changes Blacksmith value from 0 to 1 and then to 2, spawns Grey key

namespace Platformer
{
    public class BlacksmithQuest1 : BaseQuest
    {
        [SerializeField]
        private KeySpawner Spawner;

        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.Blacksmith);

            if (ProgressManager.GetQuest(EQuest.Blacksmith) > 1) return;

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                if (quest == 0)
                {
                    Player.Interaction += OnKeyQuestPhase0;
                }

                if (quest == 1)
                {
                    Player.Interaction += OnKeyQuestPhase1;
                }

                ShowMessage(Localization.Label(ELabels.Talk));
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                if (quest == 0)
                {
                    Player.Interaction -= OnKeyQuestPhase0;
                }

                if (quest == 1)
                {
                    Player.Interaction -= OnKeyQuestPhase1;
                }

                HideMessage();
            }
        }

        private void OnKeyQuestPhase0()
        {
            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Blacksmith_Title1));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Blacksmith1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith1_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith1_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Blacksmith1_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith1_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.Blacksmith, 1);
                    HideMessage();
                    Player.Interaction -= OnKeyQuestPhase0;
                    DialoguePhase = 0;
                    Inside = false;
                    break;
            }
        }

        private void OnKeyQuestPhase1()
        {
            var oreAmount = ProgressManager.GetQuest(EQuest.OreCollected);

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Blacksmith_Title1));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Blacksmith2_1));

                    if (oreAmount >= 3)
                    {
                        DialoguePhase = 5;
                    }

                    if (oreAmount < 3)
                    {
                        DialoguePhase = 10;
                    }
                    
                    break;
                case 5:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith2_2));
                    DialoguePhase++;
                    break;
                case 6:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith2_3));
                    DialoguePhase++;
                    break;
                case 7:
                    Player.ReleasedByInteraction();
                    ProgressManager.SetQuest(EQuest.Blacksmith, 2);

                    var oreRest = oreAmount - 3;
                    ProgressManager.SetQuest(EQuest.OreCollected, oreRest);
                    Game.HUD.UpdateResourceAmount();

                    ProgressManager.SetQuest(EQuest.KeyGrey, 0);
                    Spawner.SpawnItem();

                    Game.Dialogue.Hide();
                    HideMessage();
                    DialoguePhase = 0;
                    Player.Interaction -= OnKeyQuestPhase1;
                    Inside = false;
                    break;

                case 10:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith2_4));
                    DialoguePhase++;
                    break;
                case 11:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HideMessage();
                    DialoguePhase = 0;
                    Player.Interaction -= OnKeyQuestPhase1;
                    Inside = false;
                    break;
            }
        }

    }
}