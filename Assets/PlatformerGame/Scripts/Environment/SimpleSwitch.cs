using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SimpleSwitch : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private GameObject Block;

        [SerializeField]
        private Transform MessageTransform;

        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;
        private IPlayer Player;

        private SpriteRenderer Renderer;
        private MessageCanvas Message;
        private bool Interacted;
        private bool Inside;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();

            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            Renderer.sprite = Sprites[0];
            Block.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !Inside && !Interacted)
            {
                Inside = true;
                ShowMessage(Localization.Label(ELabels.SwitchOn));
                Player.Interaction += OnInteraction;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && Inside)
            {
                Inside = false;
                HideMessage();
                Player.Interaction -= OnInteraction;
            }
        }

        private void OnDisable()
        {
            Player.Interaction -= OnInteraction;
        }

        private void OnInteraction()
        {
            Renderer.sprite = Sprites[1];
            HideMessage();
            Block.SetActive(false);
            Interacted = true;
            AudioManager.PlaySound(ESounds.DoorHeavy);

            Player.Interaction -= OnInteraction;
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
    }
}