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
                ShowMessage(Localization.Text(ETexts.KindleFire));
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
            ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
            ProgressManager.SetQuest(EQuest.Location, LocationIndex);
            ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);
            SwitchFire(true);
            AudioManager.PlayRedhoodSound(EPlayerSounds.LightCampFire);

            HideMessage();
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