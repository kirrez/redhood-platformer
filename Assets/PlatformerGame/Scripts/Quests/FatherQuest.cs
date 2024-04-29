using Cinemachine;
using UnityEngine;

// Changes Suspension bridge's value from 1 to 2

namespace Platformer
{
    public class FatherQuest : BaseQuest
    {
        [SerializeField]
        private Transform TargetTransform;

        [SerializeField]
        private CrippledAxeItemSpawner Spawner;

        private CinemachineVirtualCamera PlayerCamera;

        protected override void Awake()
        {
            base.Awake();
            PlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
        }

        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.SuspensionBridge) != 1) return;

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

        private void OnInteraction()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    PlayerCamera.Follow = TargetTransform;
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.FatherDialogue_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Father_1));
                    AudioManager.PlaySound(ESounds.VoiceBeastUuu);
                    DialoguePhase++;
                    break;
                case 1:
                    PlayerCamera.Follow = Player.Transform;
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Father_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Father_3));
                    DialoguePhase++;
                    break;
                case 3:
                    PlayerCamera.Follow = TargetTransform;
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Father_4));
                    AudioManager.PlaySound(ESounds.VoiceBeastUuu);
                    DialoguePhase++;
                    break;
                case 4:
                    PlayerCamera.Follow = Player.Transform;
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Father_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Father_6));
                    AudioManager.PlaySound(ESounds.VoiceBeastUuu);
                    DialoguePhase++;
                    break;
                case 6:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Father_7));
                    DialoguePhase++;
                    //spawn axe here
                    ProgressManager.SetQuest(EQuest.CrippledAxeItem, 1);
                    Spawner.SpawnItem();
                    break;
                case 7:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Father_8));
                    DialoguePhase++;
                    break;
                case 8:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.SuspensionBridge, 2);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}