using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace Platformer
{
    public class CreditsScreenView : BaseScreenView
    {
        public event Action BackToMenuClicked = () => { };

        public NavigationButton Back;
        public NavigationButton Mode;
        public NavigationButton Forth;
        public NavigationButton BackToMenu;

        [SerializeField]
        private List<Color> TextColors;

        private ILocalization Localization;
        private IAudioManager AudioManager;

        private Action CurrentAction = () => { };
        private ESounds SelectSound = ESounds.SelectOption;

        private float Timer;
        private float ClickDelay = 0.2f;
        private bool ScrollMode;
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
            Back.SetAction(OnBack);
            Mode.SetAction(OnMode);
            Forth.SetAction(OnForth);
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
            SetButtonTexts();

            Mode.SetSound(ESounds.Silence);
            EventSystem.current.SetSelectedGameObject(Mode.gameObject);
            Mode.SelectedLook();
            Mode.SetSound(ESounds.ChooseOption);

            Back.DeselectedLook();
            Forth.DeselectedLook();
            BackToMenu.DeselectedLook();
        }

        private void SetButtonTexts()
        {
            Back.SetProperties(TextColors, "< " + Localization.Utilitary(EUtilitary.Back));
            if (ScrollMode == false)
            {
                Mode.SetProperties(TextColors, Localization.Utilitary(EUtilitary.Auto));
            }
            if (ScrollMode == true)
            {
                Mode.SetProperties(TextColors, Localization.Utilitary(EUtilitary.Manual));
            }
            
            Forth.SetProperties(TextColors, Localization.Utilitary(EUtilitary.Back) + " >");

            BackToMenu.SetProperties(TextColors, Localization.Utilitary(EUtilitary.BackToTitle));
        }

        private void BackClicked()
        {
            //
        }

        private void ModeClicked()
        {
            ScrollMode = !ScrollMode;
            SetButtonTexts();
            Mode.SubmittedLook();
        }

        private void ForthClicked()
        {
            //
        }

        private void OnBack()
        {
            Back.SubmittedLook();
            Timer = ClickDelay;
            CurrentAction = BackClicked;

            AudioManager.PlaySound(SelectSound);
        }

        private void OnMode()
        {
            Timer = ClickDelay;
            CurrentAction = ModeClicked;

            AudioManager.PlaySound(SelectSound);
        }

        private void OnForth()
        {
            Forth.SubmittedLook();
            Timer = ClickDelay;
            CurrentAction = ForthClicked;

            AudioManager.PlaySound(SelectSound);
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