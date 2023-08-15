using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class PlatformSuspended : MonoBehaviour
    {
        [SerializeField]
        Text HelpText;

        [SerializeField]
        GameObject Obstacle;

        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            HelpText.text = Localization.Text(ETexts.RemoveObstacle);

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            HelpText.gameObject.SetActive(false);
            SwitchObstacle();
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.PlatformSuspended) == 1) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                HelpText.gameObject.SetActive(true);
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HelpText.gameObject.SetActive(false);
            }
        }

        private void OnInteraction()
        {
            ProgressManager.SetQuest(EQuest.PlatformSuspended, 1);
            SwitchObstacle();

            Player.Interaction -= OnInteraction;
            HelpText.gameObject.SetActive(false);
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