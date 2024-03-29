using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Changes Suspension bridge's value from 1 to 2

namespace Platformer
{
    public class FatherQuest : MonoBehaviour
    {
        [SerializeField]
        private Transform TargetTransform;

        [SerializeField]
        private CrippledAxeItemSpawner Spawner;

        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;
        private CinemachineVirtualCamera PlayerCamera;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;
        private int DialoguePhase = 0;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
            PlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
            Player = CompositionRoot.GetPlayer();

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.SuspensionBridge) != 1) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                OnInteraction();
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.FatherDialogue));
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