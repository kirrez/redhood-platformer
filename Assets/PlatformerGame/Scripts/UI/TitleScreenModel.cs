using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class TitleScreenModel 
    {
        public event Action ClickingPlay = () => { };
        public event Action ClickingSettings = () => { };
        public event Action ClickingCredits = () => { };
        public event Action ClickingQuit = () => { };

        private TitleScreenView View;

        public TitleScreenModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<TitleScreenView, EScreens>(EScreens.TitleScreenView);
            View.SetParent(uiRoot.MenuCanvas.transform);

            View.PlayClicked += OnClickingPlay;
            View.SettingsClicked += OnClickingSettings;
            View.CreditsClicked += OnClickingCredits;
            View.QuitClicked += OnClickingQuit;
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }

        private void OnClickingPlay()
        {
            ClickingPlay?.Invoke();
        }

        private void OnClickingSettings()
        {
            ClickingSettings?.Invoke();
        }

        private void OnClickingCredits()
        {
            ClickingCredits?.Invoke();
        }

        private void OnClickingQuit()
        {
            ClickingQuit?.Invoke();
        }
    }
}