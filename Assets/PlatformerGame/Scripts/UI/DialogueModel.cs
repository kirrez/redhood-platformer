using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DialogueModel
    {
        private DialogueView View;

        public DialogueModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<DialogueView, EScreens>(EScreens.DialogueView);
            View.SetParent(uiRoot.MenuCanvas.transform);
        }

        public void SetDialogueName(string name)
        {
            View.SetDialogueName(name);
        }

        public void ChangeContent(string content)
        {
            View.ChangeContent(content);
        }

        public void AddContent(string content)
        {
            View.AddContent(content);
        }

        public void Show()
        {
            View.Enable();
        }

        public void Hide()
        {
            View.Disable();
        }
    }
}

