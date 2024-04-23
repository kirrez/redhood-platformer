using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public abstract class BaseQuest : MonoBehaviour
    {
        [SerializeField]
        protected Transform MessageTransform;

        [SerializeField]
        protected Collider2D Trigger;

        protected IResourceManager ResourceManager;
        protected IProgressManager ProgressManager;
        protected IAudioManager AudioManager;
        protected ILocalization Localization;
        protected IPlayer Player;

        protected MessageCanvas Message = null;
        protected bool Inside = false;
        protected int DialoguePhase = 0;

        protected virtual void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();
        }

        protected virtual void Update()
        {
            RequirementsCheck();
        }

        protected abstract void RequirementsCheck();

        protected void ShowMessage(string text)
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

        protected void HideMessage()
        {
            if (Message != null)
            {
                Message.gameObject.SetActive(false);
                Message = null;
            }
        }


    }
}