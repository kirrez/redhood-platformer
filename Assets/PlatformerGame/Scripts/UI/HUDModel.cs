using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HUDModel
    {
        private HUDView View;
        public HUDModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<HUDView, EScreens>(EScreens.HUDView);
            View.SetParent(uiRoot.HUDCanvas.transform);
        }

        public void SetMaxLives(int amount)
        {
            var progressManager = CompositionRoot.GetProgressManager();
            //MaxLives cannot be less than 1
            int value = Mathf.Clamp(amount, 1, progressManager.GetQuest(EQuest.MaxLivesCap));
            View.SetMaxLives(value);
        }

        public void SetCurrentLives(int amount)
        {
            var progressManager = CompositionRoot.GetProgressManager();
            //CurrentLives cannot be less than 0
            int value = Mathf.Clamp(amount, 0, progressManager.GetQuest(EQuest.MaxLivesCap));
            View.SetCurrentLives(value);
        }

        public void UpdateWeaponIcons()
        {
            View.UpdateWeaponIcons();
        }

        public void UpdateResourceAmount()
        {
            View.UpdateResourceAmount();
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