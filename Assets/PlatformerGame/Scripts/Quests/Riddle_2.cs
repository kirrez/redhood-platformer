using UnityEngine;

namespace Platformer
{
    public class Riddle_2 : BaseQuest
    {
        [SerializeField]
        private GameObject Obstacles;

        [SerializeField]
        private SpriteRenderer SwitchRenderer;

        [SerializeField]
        Collider2D SwitchTrigger;

        [SerializeField]
        private Sprite SwitchOn;

        [SerializeField]
        private Sprite SwitchOff;

        private bool TrapActivated;
        private bool SwitchTurnedOn;

        private void OnEnable()
        {
            DeactivateTrap();
            SwitchRenderer.sprite = SwitchOff;
        }

        protected override void RequirementsCheck()
        {
            if (Trigger.bounds.Contains(Player.Position) && !TrapActivated && !SwitchTurnedOn)
            {
                ActivateTrap();
            }

            if (SwitchTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Player.Interaction += OnSwitchPulled;
                Inside = true;
                ShowMessage(Localization.Text(ETexts.SwitchOn));
            }

            if (!SwitchTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Player.Interaction -= OnSwitchPulled;
                Inside = false;
                HideMessage();
            }
        }

        private void ActivateTrap()
        {
            Obstacles.SetActive(true);
            SwitchRenderer.sprite = SwitchOff;
            TrapActivated = true;

            AudioManager.PlaySound(ESounds.DoorHeavy);
        }

        private void DeactivateTrap()
        {
            Obstacles.SetActive(false);
            TrapActivated = false;
        }

        private void OnSwitchPulled()
        {
            Player.Interaction -= OnSwitchPulled;
            SwitchTurnedOn = true;
            SwitchRenderer.sprite = SwitchOn;

            AudioManager.PlaySound(ESounds.Cling4, ContainerSort.Temporary);
            HideMessage();
            DeactivateTrap();
        }
    }
}