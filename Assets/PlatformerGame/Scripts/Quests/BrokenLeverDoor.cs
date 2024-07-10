using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BrokenLeverDoor : BaseQuest
    {
        [SerializeField]
        private Collider2D LeverTrigger;

        [SerializeField]
        private GameObject DoorBlock;

        [SerializeField]
        private SpriteRenderer LeverSprite;

        [SerializeField]
        private List<Sprite> Sprites;

        private bool InsideLeverTrigger;

        delegate void State();
        State CurrentState = () => { };

        private void OnEnable()
        {
            RequirementsCheck();
        }

        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.BrokenLeverInCaves);

            if (quest == 0)
            {
                LeverSprite.sprite = Sprites[0];
                DoorBlock.SetActive(true);
                CurrentState = StatePhase0;
            }

            if (quest == 1)
            {
                LeverSprite.sprite = Sprites[0];
                DoorBlock.SetActive(true);
                CurrentState = StatePhase1;
            }

            if (quest == 2)
            {
                LeverSprite.sprite = Sprites[1];
                DoorBlock.SetActive(true);
            }

            if (quest == 3)
            {
                LeverSprite.sprite = Sprites[2];
                DoorBlock.SetActive(false);
            }
        }

        protected override void Update()
        {
            CurrentState();
        }

        private void StatePhase0()
        {
            if (Trigger.bounds.Contains(Player.Position) && Inside == false)
            {
                Inside = true;
                Player.Interaction += Consideration;
                Consideration();
            }
        }

        private void Consideration()
        {
            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.BrokenLeverInCave_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.BrokenLeverInCave1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.BrokenLeverInCaves, 1);
                    Player.Interaction -= Consideration;
                    CurrentState = StatePhase1;
                    Inside = false;
                    break;
            }
        }

        private void StatePhase1()
        {
            if (LeverTrigger.bounds.Contains(Player.Position) == true && InsideLeverTrigger == false)
            {
                InsideLeverTrigger = true;
                var leverItem = ProgressManager.GetQuest(EQuest.SwitchHandleItem);

                ShowMessage(Localization.Label(ELabels.Repair));

                if (leverItem == 0)
                {
                    Player.Interaction += HandleNotFound;
                }

                if (leverItem == 1)
                {
                    Player.Interaction += HandleFound;
                }
            }

            if (LeverTrigger.bounds.Contains(Player.Position) == false && InsideLeverTrigger == true)
            {
                InsideLeverTrigger = false;
                var leverItem = ProgressManager.GetQuest(EQuest.SwitchHandleItem);

                HideMessage();
                if (leverItem == 0)
                {
                    Player.Interaction -= HandleNotFound;
                }

                if (leverItem == 1)
                {
                    Player.Interaction -= HandleFound;
                }
            }
        }

        private void HandleNotFound()
        {
            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.BrokenLeverInCave_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.BrokenLeverInCave_HandleNotFound));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= HandleNotFound;
                    Inside = false;
                    break;
            }
        }

        private void HandleFound()
        {
            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.BrokenLeverInCave_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.BrokenLeverInCave_HandleFound));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HideMessage();
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.BrokenLeverInCaves, 2);
                    Player.Interaction -= HandleFound;
                    InsideLeverTrigger = false;
                    LeverSprite.sprite = Sprites[1];
                    CurrentState = StatePhase2;
                    Inside = false;
                    break;
            }
        }

        private void StatePhase2()
        {
            if (LeverTrigger.bounds.Contains(Player.Position) == true && InsideLeverTrigger == false)
            {
                InsideLeverTrigger = true;
                ShowMessage(Localization.Label(ELabels.PullLever));
                Player.Interaction += SwitchOn;
            }

            if (LeverTrigger.bounds.Contains(Player.Position) == false && InsideLeverTrigger == true)
            {
                InsideLeverTrigger = false;
                HideMessage();
                Player.Interaction -= SwitchOn;
            }
        }

        private void SwitchOn()
        {
            Player.HoldByInteraction();
            ProgressManager.SetQuest(EQuest.BrokenLeverInCaves, 3);
            LeverSprite.sprite = Sprites[2];
            DoorBlock.SetActive(false);
            HideMessage();
            CurrentState = () => { };

            AudioManager.PlaySound(ESounds.DoorHeavy);
            Player.ReleasedByInteraction();
            Player.Interaction -= SwitchOn;
        }
    }
}