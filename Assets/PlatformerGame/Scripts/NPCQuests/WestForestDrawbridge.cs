using UnityEngine;

namespace Platformer
{
    public class WestForestDrawbridge : MonoBehaviour
    {
        [SerializeField]
        private GameObject Drawbridge;

        [SerializeField]
        private Sprite SwitchOff;

        [SerializeField]
        private Sprite SwitchOn;

        [SerializeField]
        private Transform MessageTransform;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;

        private IPlayer Player;
        private MessageCanvas Message = null;
        private SpriteRenderer Renderer;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
            Renderer = GetComponent<SpriteRenderer>();
        }

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.Drawbridge) != 0) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                Player.Interaction += OnQuestCompleted;
                ShowMessage(Localization.Text(ETexts.PullLever));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.Drawbridge) != 0) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                if (Player != null)
                {
                    Player.Interaction -= OnQuestCompleted;
                }
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