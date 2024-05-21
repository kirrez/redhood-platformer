using UnityEngine;

namespace Platformer
{
    public class PlatformSuspended : BaseQuest
    {
        [SerializeField]
        GameObject Obstacle;

        [SerializeField]
        Transform ShattersPosition;

        private void OnEnable()
        {
            SwitchObstacle();
        }

        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.PlatformSuspended) == 1) return;

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                ShowMessage(Localization.Label(ELabels.RemoveAnObstacle));
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HideMessage();
            }
        }

        private void OnInteraction()
        {
            ProgressManager.SetQuest(EQuest.PlatformSuspended, 1);
            Player.HoldByInteraction();
            SwitchObstacle();

            Vector3 newPosition;
            for (int i = 0; i < 5; i++)
            {
                var shatter = ResourceManager.GetFromPool(GFXs.WoodShatter);
                newPosition = ShattersPosition.position;
                newPosition.y += i * 0.5f;
                shatter.GetComponent<WoodShatter>().Initiate(newPosition);
            }

            AudioManager.PlaySound(ESounds.BlastShort3);

            Player.ReleasedByInteraction();
            Player.Interaction -= OnInteraction;
            HideMessage();
        }

        private void SwitchObstacle()
        {
            if (ProgressManager.GetQuest(EQuest.PlatformSuspended) == 0)
            {
                Obstacle.SetActive(true);
            }

            if (ProgressManager.GetQuest(EQuest.PlatformSuspended) == 1)
            {
                Obstacle.SetActive(false);
            }
        }
    }
}