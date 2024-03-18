using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DestinationSign : MonoBehaviour
    {
        //[SerializeField]
        //private ETexts Label;

        [SerializeField]
        [Range(0, 5)]
        private int DestinationIndex;

        private int TargetValue;
        private ETexts Item;

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

            TargetValue = (int)ETexts.DestinationTheVillage + DestinationIndex;
            Item = (ETexts)TargetValue;
        }

        private void Update()
        {
            if (Trigger.bounds.Contains(Player.Position) && Inside == false)
            {
                ShowMessage(Localization.Text(Item));
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