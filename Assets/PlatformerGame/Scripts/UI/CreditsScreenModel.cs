using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class CreditsScreenModel
    {
        public event Action ClickingBackToMenu = () => { };

        private CreditsScreenView View;

        public CreditsScreenModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<CreditsScreenView, EScreens>(EScreens.CreditsScreenView);
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