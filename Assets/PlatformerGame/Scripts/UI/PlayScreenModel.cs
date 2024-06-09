using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class PlayScreenModel
    {
        public event Action ClickingBackToMenu = () => { };

        private PlayScreenView View;

        public PlayScreenModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<PlayScreenView, EScreens>(EScreens.PlayScreenView);
            View.SetParent(uiRoot.MenuCanvas.transform);

            View.BackToMenuClicked += OnClickingBactToMenu;
        }

        private void OnClickingBactToMenu()
        {
            ClickingBackToMenu?.Invoke();
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }
    }
}