using UnityEngine;

namespace Platformer
{
    public class Riddle_1 : BaseQuest
    {
        [SerializeField]
        private Collider2D SwitchTrigger;

        [SerializeField]
        private GameObject Obstacles;

        [SerializeField]
        private SpriteRenderer SwitchRenderer;

        [SerializeField]
        private Sprite SwitchOn;

        [SerializeField]
        private Sprite SwitchOff;
        
        private bool TrapActivated;

        private void OnEnable()
        {
            DeactivateTrap();
        }

        protected override void RequirementsCheck()
        {
            if (SwitchTrigger.bounds.Contains(Player.Position) && !Inside && TrapActivated)
            {
                Player.Interaction += OnSwitchPulled;
                Inside = true;
                ShowMessage(Localization.Text(ETexts.SwitchOn_Label));
            }

            if (!SwitchTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Player.Interaction -= OnSwitchPulled;
                Inside = false;
                HideMessage();
            }

            if (Trigger.bounds.Contains(Player.Position) && !TrapActivated)
            {
                ActivateTrap();
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
            SwitchRenderer.sprite = SwitchOn;
            TrapActivated = false;
        }

        private void OnSwitchPulled()
        {
            Player.Interaction -= OnSwitchPulled;
            SwitchRenderer.sprite = SwitchOn;

            AudioManager.PlaySound(ESounds.Cling4, ContainerSort.Temporary);
            HideMessage();
            DeactivateTrap();
        }
    }
}