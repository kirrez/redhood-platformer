using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DestinationSign : MonoBehaviour
    {
        [SerializeField]
        private ETexts Label;

        [SerializeField]
        private Transform MessageTransform;

        [SerializeField]
        private Collider2D Trigger;

        private IResourceManager ResourceManager;
        private ILocalization Localization;
        private IPlayer Player;

        private MessageCanvas Message;
        private bool Inside;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();
        }

        private void Update()
        {
            if (Trigger.bounds.Contains(Player.Position) && Inside == false)
            {
                ShowMessage(Localization.Text(Label));
                Inside = true;
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside == true)
            {
                HideMessage();
                Inside = false;
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
                Message.StopBlinking();
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