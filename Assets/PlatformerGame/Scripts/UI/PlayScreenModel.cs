using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class PlayScreenModel
    {
        public event Action ClickingBackToMenu = () => { };
        public event Action ClickingPlayGame = () => { };

        private PlayScreenView View;

        public PlayScreenModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<PlayScreenView, EScreens>(EScreens.PlayScreenView);
            View.SetParent(uiRoot.MenuCanvas.transform);

            View.BackToMenuClicked += OnClickingBackToMenu;
            View.PlayGameClicked += OnClickingPlayGame;
        }

        private void OnClickingBackToMenu()
        {
            ClickingBackToMenu?.Invoke();
        }

        private void OnClickingPlayGame()
        {
            ClickingPlayGame?.Invoke();
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