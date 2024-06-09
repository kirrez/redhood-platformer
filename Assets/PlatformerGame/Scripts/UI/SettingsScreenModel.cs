using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class SettingsScreenModel
    {
        public event Action ClickingBackToMenu = () => { };

        private SettingsScreenView View;

        public SettingsScreenModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<SettingsScreenView, EScreens>(EScreens.SettingsScreenView);
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