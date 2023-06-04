using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class DialogueView : BaseScreenView
    {
        [SerializeField]
        private Text DialogueName;
        [SerializeField]
        private Text Content;
        [SerializeField]
        private Text Tip;

        private ILocalization Localization;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
        }

        private void OnEnable()
        {
            Tip.text = Localization.Text(ETexts.Next);
        }

        public void SetDialogueName(string name)
        {
            DialogueName.text = name;
        }

        public void ChangeContent(string content)
        {
            Content.text = content;
        }

        public void AddContent(string content)
        {
            Content.text = Content.text + "\n" + content;
        }
    }
}

