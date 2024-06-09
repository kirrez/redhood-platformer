using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace Platformer
{
    public class TitleScreenView : BaseScreenView
    {
        public event Action PlayClicked = () => { };
        public event Action SettingsClicked = () => { };
        public event Action CreditsClicked = () => { };
        public event Action QuitClicked = () => { };

        public MenuButton Play;
        public MenuButton Settings;
        public MenuButton Credits;
        public MenuButton Quit;

        public NavigationButton English;
        public NavigationButton Russian;

        [SerializeField]
        private List<Color> ButtonTextColors; //0 - selected, 1 - deselected, 2 - pressed

        [SerializeField]
        private List<Color> LanguageTextColors;

        private ILocalization Localization;
        private IAudioManager AudioManager;

        private Action CurrentAction = () => { };

        private float Timer;
        private float ClickDelay = 0.5f;
        private ESounds SelectSound = ESounds.SelectOption;
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

        private void Update()
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    SwitchButtons(true);
                    CurrentAction();
                }
            }
        }

        private void Start()
        {
            ChangeLocalization(ELocalizations.English);

            Play.SetAction(OnPlayClicked);
            Settings.SetAction(OnSettingsClicked);
            Credits.SetAction(OnCreditsClicked);
            Quit.SetAction(OnQuitClicked);
            English.SetAction(OnEnglishClicked);
            Russian.SetAction(OnRussianClicked);

            SwitchButtons(true);
            AudioManager.PlayMusic(EMusic.TheVillage_Night);

            ScreenCreated = true;

            ResetSelectedView();
        }

        private void ResetSelectedView()
        {
            Play.SetSound(ESounds.Silence);
            EventSystem.current.SetSelectedGameObject(Play.gameObject);
            Play.SelectedLook();
            Play.SetSound(ESounds.ChooseOption);

            Settings.DeselectedLook();
            Credits.DeselectedLook();
            Quit.DeselectedLook();
        }

        private void ChangeLocalization(ELocalizations localization)
        {
            Localization.LoadLocalization(localization);

            Play.SetProperties(ButtonTextColors, Localization.Utilitary(EUtilitary.Play_selected), Localization.Utilitary(EUtilitary.Play_deselected));
            Settings.SetProperties(ButtonTextColors, Localization.Utilitary(EUtilitary.Settings_selected), Localization.Utilitary(EUtilitary.Settings_deselected));
            Credits.SetProperties(ButtonTextColors, Localization.Utilitary(EUtilitary.Credits_selected), Localization.Utilitary(EUtilitary.Credits_deselected));
            Quit.SetProperties(ButtonTextColors, Localization.Utilitary(EUtilitary.Quit_selected), Localization.Utilitary(EUtilitary.Quit_deselected));
            English.SetProperties(LanguageTextColors, Localization.Utilitary(EUtilitary.English));
            Russian.SetProperties(LanguageTextColors, Localization.Utilitary(EUtilitary.Russian));
        }

        private void SwitchButtons(bool flag)
        {
            Play.SetInteractable(flag);
            Settings.SetInteractable(flag);
            Credits.SetInteractable(flag);
            Quit.SetInteractable(flag);
            English.SetInteractable(flag);
            Russian.SetInteractable(flag);
        }

        private void OnEnglishClicked()
        {
            if (Localization.GetLocalization() != ELocalizations.English)
            {
                ChangeLocalization(ELocalizations.English);
            }

            AudioManager.PlaySound(SelectSound);
        }

        private void OnRussianClicked()
        {
            if (Localization.GetLocalization() != ELocalizations.Russian)
            {
                ChangeLocalization(ELocalizations.Russian);
            }

            AudioManager.PlaySound(SelectSound);
        }

        private void OnPlayClicked()
        {
            Play.SubmittedLook();
            Timer = ClickDelay;
            SwitchButtons(false);
            CurrentAction = PlayClicked;

            AudioManager.PlaySound(SelectSound);
        }

        private void OnSettingsClicked()
        {
            Settings.SubmittedLook();
            Timer = ClickDelay;
            SwitchButtons(false);
            CurrentAction = SettingsClicked;

            AudioManager.PlaySound(SelectSound);
        }

        private void OnCreditsClicked()
        {
            Credits.SubmittedLook();
            Timer = ClickDelay;
            SwitchButtons(false);
            CurrentAction = CreditsClicked;

            AudioManager.PlaySound(SelectSound);
        }

        private void OnQuitClicked()
        {
            Quit.SubmittedLook();
            Timer = ClickDelay;
            SwitchButtons(false);
            CurrentAction = QuitClicked;

            AudioManager.PlaySound(SelectSound);
        }
    }
}