using System;

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
            View.Enable();
        }

        public void Hide()
        {
            View.Disable();
        }

        private void OnTryAgainClicked()
        {
            TryingAgain();
        }
    }
}