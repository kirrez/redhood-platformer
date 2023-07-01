using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class GameOverView : BaseScreenView
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
            TryAgainText.text = localization.Text(ETexts.TryAgain);
        }
    }
}