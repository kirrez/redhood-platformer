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

        [SerializeField]
        Transform ShattersPosition;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            HelpText.text = Localization.Text(ETexts.RemoveObstacle);
            HelpText.gameObject.SetActive(false);

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
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

            Vector3 newPosition;
            for (int i = 0; i < 5; i++)
            {
                var shatter = ResourceManager.GetFromPool(GFXs.WoodShatter);
                newPosition = ShattersPosition.position;
                newPosition.y += i * 0.5f;
                shatter.GetComponent<WoodShatter>().Initiate(newPosition);
            }

            AudioManager.PlaySound(ESounds.BlastShort3);

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