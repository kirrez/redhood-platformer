using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class CampFire : BaseQuest
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
                ShowMessage(Localization.Label(ELabels.KindleAFire));
            }

            if (Trigger.bounds.Contains(Player.Position) == false && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HideMessage();
            }
            //SwitchFire(false); //every frame..
        }

        private void OnInteraction()
        {
            Player.HoldByInteraction();
            ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
            ProgressManager.SetQuest(EQuest.Location, LocationIndex);
            ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);

            Storage.Save(ProgressManager.PlayerState);

            SwitchFire(true);
            AudioManager.PlayRedhoodSound(EPlayerSounds.LightCampFire);
            Player.UpdateMaxLives(); //refills health and updates HUD..

            HideMessage();
            Player.ReleasedByInteraction();
            Player.Interaction -= OnInteraction;
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