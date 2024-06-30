using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CursedCampFire : BaseQuest
    {
        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int ConfinerIndex;

        [SerializeField]
        private Sprite FireOff;

        [SerializeField]
        private Sprite FireOn;

        [SerializeField]
        private GameObject Fire;

        [SerializeField]
        private SpriteRenderer Renderer;

        private IStorage Storage;

        protected override void Awake()
        {
            base.Awake();
            Storage = CompositionRoot.GetStorage();
        }

        private void OnEnable()
        {
            Inside = false;
            DialoguePhase = 0;

            if (ProgressManager.GetQuest(EQuest.SpawnPoint) == SpawnPointIndex)
            {
                SwitchFire(true);
            }
            else
            {
                SwitchFire(false);
            }
        }

        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.SpawnPoint) == SpawnPointIndex) return;

            if (Trigger.bounds.Contains(Player.Position) == true && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                // instant beginning
                OnInteraction();
            }

            if (Trigger.bounds.Contains(Player.Position) == false && Inside)
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.ForsakenCamp_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.ForsakenCamp_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();

                    ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
                    ProgressManager.SetQuest(EQuest.Location, LocationIndex);
                    ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);

                    Storage.Save(ProgressManager.PlayerState);

                    SwitchFire(true);
                    AudioManager.PlayRedhoodSound(EPlayerSounds.LightCursedCampFire);

                    Player.Interaction -= OnInteraction;
                    break;
            }
        }

        private void SwitchFire(bool active)
        {
            if (active)
            {
                Renderer.sprite = FireOn;
                Fire.SetActive(true);
            }

            if (!active)
            {
                Renderer.sprite = FireOff;
                Fire.SetActive(false);
            }
        }
    }
}