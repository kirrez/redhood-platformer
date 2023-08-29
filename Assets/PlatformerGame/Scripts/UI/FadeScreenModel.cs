using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FadeScreenModel
    {
        private FadeScreenView View;

        public FadeScreenModel()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            var uiRoot = CompositionRoot.GetUIRoot();

            View = resourceManager.CreatePrefab<FadeScreenView, EScreens>(EScreens.FadeScreenView);
            View.SetParent(uiRoot.MenuCanvas.transform);
        }

        public void FadeIn(Color color, float time)
        {
            View.FadeIn(color, time);
        }

        public void FadeOut(Color color, float time)
        {
            View.FadeOut(color, time);
        }

        public void DelayAfter(float delay)
        {
            View.DelayAfter(delay);
        }

        public void DelayBefore(Color color, float time)
        {
            View.DelayBefore(color, time);
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