using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class AssistantMegafrog : BaseQuest
    {
        [SerializeField]
        private GameObject Character;

        private int PoisonPrice = 100;

        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.AssistantMegafrog);
            var megafrog = ProgressManager.GetQuest(EQuest.Megafrog);
            var money = ProgressManager.GetQuest(EQuest.Money);

            if (quest == 3 && megafrog == 1)
            {
                ProgressManager.SetQuest(EQuest.AssistantMegafrog, 4);
            }

            if (Trigger.bounds.Contains(Player.Position) == true && !Inside)
            {
                Inside = true;
                ShowMessage(Localization.Text(ETexts.Talk));

                if (quest == 0)
                {
                    Player.Interaction += FirstReplica;
                }

                if (quest > 0 && quest != 4 && megafrog == 1)
                {
                    Player.Interaction += LastNoAssistance;
                }

                if (quest == 1 && megafrog == 0)
                {
                    Player.Interaction += SecondReplica;
                }

                if (quest == 2 && megafrog == 0)
                {
                    if (money >= PoisonPrice)
                    {
                        Player.Interaction += DealAccepted;
                    }
                    else
                    {
                        Player.Interaction += DealDeclined;
                    }
                }

                if (quest == 3 && megafrog == 0)
                {
                    Player.Interaction += PoisonedButAlive;
                }

                if (quest == 4)
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

                if (quest > 0 && quest != 4 && megafrog == 1)
                {
                    Player.Interaction -= LastNoAssistance;
                }

                if (quest == 2 && megafrog == 0)
                {
                    if (money >= PoisonPrice)
                    {
                        Player.Interaction -= DealAccepted;
                    }
                    else
                    {
                        Player.Interaction -= DealDeclined;
                    }
                }

                if (quest == 3 && megafrog == 0)
                {
                    Player.Interaction -= PoisonedButAlive;
                }

                if (quest == 4)
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
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_frog1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.AssistantMegafrog, 1);
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
                    string replica = Localization.Text(ETexts.Assistant_frog2_1) + PoisonPrice.ToString() + Localization.Text(ETexts.Assistant_frog2_2);
                    Game.Dialogue.ChangeContent(replica);
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.AssistantMegafrog, 2);
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
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_frog3));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.AssistantMegafrog, 3);

                    var health = Mathf.CeilToInt(ProgressManager.GetQuest(EQuest.MegafrogMaxHealth) / 2);
                    ProgressManager.SetQuest(EQuest.MegafrogMaxHealth, health);

                    ProgressManager.AddValue(EQuest.Money, -PoisonPrice);
                    Game.HUD.UpdateResourceAmount(); // show new money

                    AudioManager.PlaySound(ESounds.CursedCampFire);

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
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_frog4));
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

        private void PoisonedButAlive()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Assistant_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_frog5));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    HideMessage();
                    Player.Interaction -= PoisonedButAlive;
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
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_frog6));
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
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Assistant_frog7));
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