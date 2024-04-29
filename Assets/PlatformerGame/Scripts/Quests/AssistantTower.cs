using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class AssistantTower : BaseQuest
    {
        [SerializeField]
        private GameObject Character;

        [SerializeField]
        private PlatformActivation PlatformActivation;

        private int PassPrice = 250;

        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.AssistantTower);
            var elevator = ProgressManager.GetQuest(EQuest.WFSecondElevator);
            var money = ProgressManager.GetQuest(EQuest.Money);

            if (Trigger.bounds.Contains(Player.Position) == true && !Inside)
            {
                Inside = true;
                ShowMessage(Localization.Text(ETexts.Talk_Label));

                if (quest == 0)
                {
                    Player.Interaction += FirstReplica;
                }

                if (quest > 0 && quest != 3 && elevator == 1)
                {
                    Player.Interaction += LastNoAssistance;
                }

                if (quest == 1 && elevator == 0)
                {
                    Player.Interaction += SecondReplica;
                }

                if (quest == 2 && elevator == 0)
                {
                    if (money >= PassPrice)
                    {
                        Player.Interaction += DealAccepted;
                    }
                    else
                    {
                        Player.Interaction += DealDeclined;
                    }
                }

                if (quest == 3)
                {
                    Player.Interaction += LastWithAssistance;
                }
            }

            if (Trigger.bounds.Contains(Player.Position) == false && Inside)
            {
                Inside = false;
                HideMessage();

                if (quest == 0)
                {
                    Player.Interaction -= FirstReplica;
                }

                if (quest > 0 && quest != 3 && elevator == 1)
                {
                    Player.Interaction -= LastNoAssistance;
                }

                if (quest == 2 && elevator == 0)
                {
                    if (money >= PassPrice)
                    {
                        Player.Interaction -= DealAccepted;
                    }
                    else
                    {
                        Player.Interaction -= DealDeclined;
                    }
                }

                if (quest == 3)
                {
                    Player.Interaction -= LastWithAssistance;
                }
            }
        }

        private void FirstReplica()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Assistant_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_tower1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.AssistantTower, 1);
                    Player.Interaction -= FirstReplica;
                    break;
            }
        }

        private void SecondReplica()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Assistant_Title));
                    string replica = Localization.Text(ETexts.Assistant_tower2_1) + PassPrice.ToString() + Localization.Text(ETexts.Assistant_tower2_2);
                    Game.Dialogue.ChangeContent(replica);
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.AssistantTower, 2);
                    Player.Interaction -= SecondReplica;
                    break;
            }
        }

        private void DealAccepted()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Assistant_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_tower3));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    PlatformActivation.Activate();
                    ProgressManager.SetQuest(EQuest.AssistantTower, 3);
                    ProgressManager.AddValue(EQuest.Money, -PassPrice);
                    Game.HUD.UpdateResourceAmount(); // show new money

                    HideMessage();
                    Player.Interaction -= DealAccepted;
                    break;
            }
        }

        private void DealDeclined()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Assistant_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_tower4));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    HideMessage();
                    Player.Interaction -= DealDeclined;
                    break;
            }
        }

        private void LastWithAssistance()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Assistant_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_tower5));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    HideMessage();
                    Player.Interaction -= LastWithAssistance;
                    break;
            }
        }

        private void LastNoAssistance()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Assistant_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_tower6));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    HideMessage();
                    Player.Interaction -= LastNoAssistance;
                    break;
            }
        }
    }
}