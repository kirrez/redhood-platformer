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

        private IAudioManager AudioManager;
        private ILocalization Localization;
        private IMessageCanvas Message;
        private IPlayer Player;

        private SpriteRenderer Renderer;
        private bool Interacted;
        private bool Inside;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
            Message = CompositionRoot.GetMessageCanvas();
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
            Message.Show();
            Message.SetPosition(MessageTransform.position);
            Message.SetMessage(text);
            Message.SetBlinking(true, 0.5f);
        }

        private void HideMessage()
        {
            Message.Hide();
        }
    }
}