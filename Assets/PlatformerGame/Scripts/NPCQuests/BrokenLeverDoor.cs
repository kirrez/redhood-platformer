using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BrokenLeverDoor : MonoBehaviour
    {
        [SerializeField]
        private Collider2D QuestTrigger;

        [SerializeField]
        private Collider2D LeverTrigger;

        [SerializeField]
        private Transform MessageTransform;

        [SerializeField]
        private GameObject DoorBlock;

        [SerializeField]
        private SpriteRenderer LeverSprite;

        [SerializeField]
        private List<Sprite> Sprites;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;
        private IPlayer Player;

        private MessageCanvas Message = null;
        private bool IsInsideQuestTrigger;
        private bool IsInsideLeverTrigger;
        private int DialoguePhase = 0;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
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

        private void Update()
        {
            CurrentState();
        }

        private void ShowMessage(string text)
        {
            if (Message == null)
            {
                var instance = ResourceManager.GetFromPool(EComponents.MessageCanvas);
                Message = instance.GetComponent<MessageCanvas>();
                Message.SetPosition(MessageTransform.position);
                Message.SetMessage(text);
                Message.SetBlinking(true, 0.5f);
            }
        }

        private void HideMessage()
        {
            if (Message != null)
            {
                Message.gameObject.SetActive(false);
                Message = null;
            }
        }

        private void StatePhase0()
        {
            if (QuestTrigger.bounds.Contains(Player.Position) && IsInsideQuestTrigger == false)
            {
                IsInsideQuestTrigger = true;
                Player.Interaction += Consideration;
                Consideration();
            }
        }

        private void Consideration()
        {
            var Game = CompositionRoot.GetGame();

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
                    break;
            }
        }

        private void StatePhase1()
        {
            if (LeverTrigger.bounds.Contains(Player.Position) == true && IsInsideLeverTrigger == false)
            {
                IsInsideLeverTrigger = true;
                var leverItem = ProgressManager.GetQuest(EQuest.SwitchHandleItem);

                ShowMessage(Localization.Text(ETexts.Repair));

                if (leverItem == 0)
                {
                    Player.Interaction += HandleNotFound;
                }

                if (leverItem == 1)
                {
                    Player.Interaction += HandleFound;
                }
            }

            if (LeverTrigger.bounds.Contains(Player.Position) == false && IsInsideLeverTrigger == true)
            {
                IsInsideLeverTrigger = false;
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
            var Game = CompositionRoot.GetGame();

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
                    Message.SetBlinking(true, 0.5f);
                    Player.Interaction -= HandleNotFound;
                    break;
            }
        }

        private void HandleFound()
        {
            var Game = CompositionRoot.GetGame();

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
                    DialoguePhase = 0;

                    ProgressManager.SetQuest(EQuest.BrokenLeverInCaves, 2);
                    Player.Interaction -= HandleFound;
                    IsInsideLeverTrigger = false;
                    LeverSprite.sprite = Sprites[1];
                    CurrentState = StatePhase2;
                    break;
            }
        }

        private void StatePhase2()
        {
            if (LeverTrigger.bounds.Contains(Player.Position) == true && IsInsideLeverTrigger == false)
            {
                IsInsideLeverTrigger = true;
                ShowMessage(Localization.Text(ETexts.PullLever));
                Player.Interaction += SwitchOn;
            }

            if (LeverTrigger.bounds.Contains(Player.Position) == false && IsInsideLeverTrigger == true)
            {
                IsInsideLeverTrigger = false;
                HideMessage();
                Player.Interaction -= SwitchOn;
            }
        }

        private void SwitchOn()
        {
            ProgressManager.SetQuest(EQuest.BrokenLeverInCaves, 3);
            LeverSprite.sprite = Sprites[2];
            DoorBlock.SetActive(false);
            HideMessage();
            CurrentState = () => { };

            AudioManager.PlaySound(ESounds.DoorHeavy);
            Player.Interaction -= SwitchOn;
        }
    }
}