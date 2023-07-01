using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Platformer
{
    public class GameOverModel
    {
        public event Action TryingAgain = () => { };

        private GameOverView View;

        public GameOverModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<GameOverView, EScreens>(EScreens.GameOverView);
            View.SetParent(uiRoot.MenuCanvas.transform);

            View.TryAgainClicked += OnTryAgainClicked;
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }

        private void OnTryAgainClicked()
        {
            TryingAgain();
        }
    }
}