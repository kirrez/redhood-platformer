using UnityEngine;

namespace Platformer
{
    public class Riddle_1 : MonoBehaviour
    {
        [SerializeField]
        private GameObject Obstacles;

        [SerializeField]
        private SpriteRenderer SwitchRenderer;

        [SerializeField]
        Collider2D SwitchCollider;

        [SerializeField]
        private Sprite SwitchOn;

        [SerializeField]
        private Sprite SwitchOff;

        [SerializeField]
        private Transform MessageTransform;

        private IResourceManager ResourceManager;
        private ILocalization Localization;
        private IAudioManager AudioManager;
        private IPlayer Player;

        private MessageCanvas Message;
        private bool TrapActivated;
        private bool InsideSwitch;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Localization = CompositionRoot.GetLocalization();
        }

        private void OnEnable()
        {
            DeactivateTrap();
        }

        private void Update()
        {
            if (Player == null) return;

            if (SwitchCollider.bounds.Contains(Player.Position) && !InsideSwitch && TrapActivated)
            {
                Player.Interaction += OnSwitchPulled;
                InsideSwitch = true;
                ShowMessage(Localization.Text(ETexts.SwitchOn));
            }

            if (!SwitchCollider.bounds.Contains(Player.Position) && InsideSwitch)
            {
                Player.Interaction -= OnSwitchPulled;
                InsideSwitch = false;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !TrapActivated)
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
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