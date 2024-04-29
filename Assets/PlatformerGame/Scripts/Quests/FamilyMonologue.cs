using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Changes Mother's pie value from 1 to 2

namespace Platformer
{
    public class FamilyMonologue : BaseQuest
    {
        [SerializeField]
        Transform TargetTransform;

        private CinemachineVirtualCamera PlayerCamera;

        protected override void Awake()
        {
            base.Awake();
            PlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
        }

        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 1) return;

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                // instant beginning
                OnInteraction();
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.FamilyMonologue_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Family_1));
                    DialoguePhase++;
                    break;
                case 1:
                    PlayerCamera.Follow = TargetTransform;
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Family_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Family_3));
                    DialoguePhase++;
                    break;
                case 3:
                    PlayerCamera.Follow = Player.Transform;
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Family_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.MotherPie, 2);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}