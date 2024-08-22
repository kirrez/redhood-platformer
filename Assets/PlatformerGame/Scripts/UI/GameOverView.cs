using System;
using UnityEngine.UI;

namespace Platformer
{
    public class GameOverView : BaseView
    {
        public event Action TryAgainClicked = () => {};

        public Button TryAgainButton;

        public Text TryAgainText;

        public void OnTryAgainClick()
        {
            TryAgainClicked();
        }

        private void Awake()
        {
            var localization = CompositionRoot.GetLocalization();

            TryAgainButton.onClick.AddListener(OnTryAgainClick);
            TryAgainText.text = localization.Utilitary(EUtilitary.TryAgain);
        }
    }
}