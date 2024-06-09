using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace Platformer
{
    public class SettingsScreenView : BaseScreenView
    {
        public event Action BackToMenuClicked = () => { };

        // Buttons for various settings

        public NavigationButton BackToMenu;

        [SerializeField]
        private List<Color> TextColors;

        private ILocalization Localization;
        private IAudioManager AudioManager;

        private Action CurrentAction = () => { };
        private ESounds SelectSound = ESounds.SelectOption;

        private float Timer;
        private float ClickDelay = 0.2f;
        private bool ScreenCreated;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
        }

        private void OnEnable()
        {
            if (ScreenCreated == true)
            {
                ResetSelectedView();
            }
        }

        private void Start()
        {
            BackToMenu.SetAction(OnBackToMenu);

            ScreenCreated = true;
            ResetSelectedView();
        }

        private void Update()
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    CurrentAction();
                }
            }
        }

        private void ResetSelectedView()
        {
            SetButtonsText();

            BackToMenu.SetSound(ESounds.Silence);
            EventSystem.current.SetSelectedGameObject(BackToMenu.gameObject);
            BackToMenu.SelectedLook();
            BackToMenu.SetSound(ESounds.ChooseOption);
        }

        private void SetButtonsText()
        {
            BackToMenu.SetProperties(TextColors, Localization.Utilitary(EUtilitary.BackToTitle));
        }

        private void OnBackToMenu()
        {
            BackToMenu.SubmittedLook();
            Timer = ClickDelay;
            CurrentAction = BackToMenuClicked;

            AudioManager.PlaySound(SelectSound);
        }
    }
}