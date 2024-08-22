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

        protected IMessageCanvas Message;

        protected bool Inside = false;
        protected int DialoguePhase = 0;

        protected virtual void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            Message = CompositionRoot.GetMessageCanvas();
        }

        protected virtual void Update()
        {
            RequirementsCheck();
        }

        protected abstract void RequirementsCheck();

        protected void ShowMessage(string text)
        {
            Message.Show();
            Message.SetPosition(MessageTransform.position);
            Message.SetMessage(text);
            Message.SetBlinking(true, 0.5f);
        }

        protected void HideMessage()
        {
            Message.Hide();
        }

    }
}