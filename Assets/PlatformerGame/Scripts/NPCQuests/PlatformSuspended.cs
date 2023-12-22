using UnityEngine;

namespace Platformer
{
    public class PlatformSuspended : MonoBehaviour
    {
        [SerializeField]
        GameObject Obstacle;

        [SerializeField]
        Transform ShattersPosition;

        [SerializeField]
        private Transform MessageTransform;

        private IProgressManager ProgressManager;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;
        private IPlayer Player;

        private MessageCanvas Message = null;
        private Collider2D AreaTrigger;
        private bool Inside = false;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

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
                ShowMessage(Localization.Text(ETexts.RemoveObstacle));
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HideMessage();
            }
        }

        private void ShowMessage(string text)
        {
            if (Message == null)
            {
                var instance = ResourceManager.GetFromPool(EComponents.MessageCanvas);
                Message = instance.GetComponent<MessageCanvas>();
                Message.SetPosition(MessageTransform.position);
                Message.SetMessage(text);
                Message.SetBlinking(true, 0.5f);
            }
        }

        private void HideMessage()
        {
            if (Message != null)
            {
                Message.gameObject.SetActive(false);
                Message = null;
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