using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class FamilyMonologue : MonoBehaviour
    {
        [SerializeField]
        Transform TargetTransform;

        private IProgressManager ProgressManager;
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
            PlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
            Player = CompositionRoot.GetPlayer();

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 1) return;

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
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.FamilyMonologue));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Family_1));
                    DialoguePhase++;
                    break;
                case 1:
                    PlayerCamera.Follow = TargetTransform;
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Family_2));
                    DialoguePhase++;
                    break;
                case 2:
                    
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Family_3));
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