using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DestinationSign : MonoBehaviour
    {
        [SerializeField]
        private EDestinationNames Name;

        [SerializeField]
        private Transform MessageTransform;

        [SerializeField]
        private Collider2D Trigger;

        private ILocalization Localization;
        private IMessageCanvas Message;
        private IPlayer Player;

        private bool Inside;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            Message = CompositionRoot.GetMessageCanvas();

            Player = CompositionRoot.GetPlayer();
        }

        private void Update()
        {
            if (Trigger.bounds.Contains(Player.Position) && Inside == false)
            {
                ShowMessage(Localization.Destination(Name));
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
            Message.Show();
            Message.SetPosition(MessageTransform.position);
            Message.SetMessage(text);
            Message.StopBlinking();
        }

        private void HideMessage()
        {
            Message.Hide();
        }

    }
}