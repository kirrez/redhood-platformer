using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HUDModel
    {
        private HUDView View;

        private IProgressManager ProgressManager;
        public HUDModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            ProgressManager = CompositionRoot.GetProgressManager();

            View = resourceManager.CreatePrefab<HUDView, EScreens>(EScreens.HUDView);
            View.SetParent(uiRoot.HUDCanvas.transform);
        }

        public void SetMaxLives(int amount)
        {
            //MaxLives cannot be less than 1
            int value = Mathf.Clamp(amount, 1, ProgressManager.GetQuest(EQuest.MaxLivesCap));
            View.SetMaxLives(value);
        }

        public void SetCurrentLives(int amount)
        {
            //CurrentLives cannot be less than 0
            int value = Mathf.Clamp(amount, 0, ProgressManager.GetQuest(EQuest.MaxLivesCap));
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

        public void ShowEnemyHealthBar()
        {
            View.ShowEnemyHealthBar();
        }

        public void HideEnemyHealthBar()
        {
            View.HideEnemyHealthBar();
        }

        public void SetEnemyIcon(int index)
        {
            View.SetEnemyIcon(index);
        }

        public void SetEnemyMaxHealth(int health)
        {
            View.SetEnemyMaxHealth(health);
        }

        public void SetEnemyCurrentHealth(int health)
        {
            View.SetEnemyCurrentHealth(health);
        }


        //--------------------------
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