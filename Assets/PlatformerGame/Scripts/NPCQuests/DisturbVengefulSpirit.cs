using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DisturbVengefulSpirit : MonoBehaviour
    {
        [SerializeField]
        private VengefulSpiritSpawner Spawner;

        [SerializeField]
        private BatSpawner BatSpawner;

        [SerializeField]
        private Collider2D AreaTrigger;

        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;
        private IPlayer Player;

        private bool IsInside = false;
        private int DialoguePhase = 0;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            int spirit = ProgressManager.GetQuest(EQuest.VengefulSpiritDisturbed);

            if (spirit == 0)
            {
                Spawner.gameObject.SetActive(false);
                BatSpawner.gameObject.SetActive(true);
            }

            if (spirit == 1)
            {
                Spawner.gameObject.SetActive(true);
                BatSpawner.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            int water = ProgressManager.GetQuest(EQuest.HolyWaterLevel);
            int spirit = ProgressManager.GetQuest(EQuest.VengefulSpiritDisturbed);

            if (water > 0 && spirit == 0)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && IsInside == false)
                {
                    IsInside = true;
                    Player.Interaction += SpiritAppears;
                    SpiritAppears();
                }
            }
        }

        private void SpiritAppears()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.DisturbSpirit_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DisturbSpirit1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DisturbSpirit2));
                    DialoguePhase++;
                    break;
                case 2:
                    AudioManager.PlaySound(ESounds.VoiceEvilLaughter);
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DisturbSpirit3));
                    DialoguePhase++;
                    break;
                case 3:
                    BatSpawner.gameObject.SetActive(false);
                    Spawner.gameObject.SetActive(true);
                    Spawner.Spawn();
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DisturbSpirit4));
                    DialoguePhase++;
                    break;
                case 4:

                    ProgressManager.SetQuest(EQuest.VengefulSpiritDisturbed, 1);
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    Player.Interaction -= SpiritAppears;
                    break;
            }
        }

    }
}