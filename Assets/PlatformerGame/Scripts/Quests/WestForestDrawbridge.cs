using UnityEngine;

namespace Platformer
{
    public class WestForestDrawbridge : BaseQuest
    {
        [SerializeField]
        private GameObject Drawbridge;

        [SerializeField]
        private Sprite SwitchOff;

        [SerializeField]
        private Sprite SwitchOn;

        [SerializeField]
        private SpriteRenderer Renderer;

        private void OnEnable()
        {
            if (ProgressManager.GetQuest(EQuest.Drawbridge) == 0)
            {
                Renderer.sprite = SwitchOff;
                Drawbridge.SetActive(false);
            }

            if (ProgressManager.GetQuest(EQuest.Drawbridge) !=0)
            {
                Renderer.sprite = SwitchOn;
            }
        }

        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.Drawbridge) != 0) return;

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnQuestCompleted;
                ShowMessage(Localization.Text(ETexts.PullLever_Label));
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnQuestCompleted;
                HideMessage();
            }
        }

        private void OnQuestCompleted()
        {
            ProgressManager.SetQuest(EQuest.Drawbridge, 1);
            HideMessage();
            Player.Interaction -= OnQuestCompleted;

            Drawbridge.SetActive(true);
            Renderer.sprite = SwitchOn;

            AudioManager.PlaySound(ESounds.DoorHeavy);
        }
    }
}