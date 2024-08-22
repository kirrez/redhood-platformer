using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{
    public class PlayScreenView : BaseScreenView
    {
        public event Action BackToMenuClicked = () => { };
        public event Action PlayGameClicked = () => { };

        private const string NoName = "NoName";

        public Text Title;
        public NavigationButton BackToMenu;

        [SerializeField]
        private List<Color> TextColors;

        [SerializeField]
        private List<GameSlot> Slots;

        [SerializeField]
        private SubmitButton CreatePlayButton;

        [SerializeField]
        private SubmitButton RenameButton;

        [SerializeField]
        private SubmitButton DeleteButton;

        [Header("Input Name Window")]
        [Space(20)]

        [SerializeField]
        private GameObject InputNameWindow;

        [SerializeField]
        private InputField NameInput;

        [SerializeField]
        private Text InputWindowTitle;

        [SerializeField]
        private Text SubmitText;

        [SerializeField]
        private Text CancelText;

        [SerializeField]
        private Button SubmitInputName;

        [SerializeField]
        private Button CancelInputName;

        [Header("Deletion Window")]
        [Space(20)]

        [SerializeField]
        private GameObject DeletionWindow;

        [SerializeField]
        private Text DeletionWindowTitle;

        [SerializeField]
        private Text SubmitDeletionText;

        [SerializeField]
        private Text CancelDeletionText;

        [SerializeField]
        private Button SubmitDeletion;

        [SerializeField]
        private Button CancelDeletion;

        //----- DEBUG --------------------

        [SerializeField]
        private Text SelectedIDText;

        //--------------------------------
        private ILocalization Localization;
        private IAudioManager AudioManager;

        private Action CurrentAction = () => { };
        private ESounds SelectSound = ESounds.SelectOption;

        private float Timer;
        private float ClickDelay = 0.35f;
        private bool ScreenCreated;

        private IStorage Storage;

        private IPlayerState[] PlayerStates = new IPlayerState[3];

        private IProgressManager ProgressManager;

        private int SelectedSlotID = -1;

        private void Awake()
        {
            Storage = CompositionRoot.GetStorage();
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();

            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnEnable()
        {
            if (ScreenCreated == true)
            {
                ResetButtonTexts();
                ResetSelectedView();

                SelectedSlotID = -1;

                foreach (var slot in Slots)
                {
                    slot.SelectSlot(false);
                    slot.DeselectedLook();
                }

                CreatePlayButton.SetAvailable(false);
                RenameButton.SetAvailable(false);
                DeleteButton.SetAvailable(false);

                SwitchMainLayer(true);
            }
        }

        private void Start()
        {
            if (Storage.IsPlayerStateExists(0))
            {
                PlayerStates[0] = Storage.LoadPlayerState(0);
            }

            if (Storage.IsPlayerStateExists(1))
            {
                PlayerStates[1] = Storage.LoadPlayerState(1);
            }

            if (Storage.IsPlayerStateExists(2))
            {
                PlayerStates[2] = Storage.LoadPlayerState(2);
            }

            Slots[0].SetProperties(0, PlayerStates[0]);
            Slots[1].SetProperties(1, PlayerStates[1]);
            Slots[2].SetProperties(2, PlayerStates[2]);

            foreach (var slot in Slots)
            {
                slot.SendID += OnSendID;
                slot.SetAction(slot.PerformSubmit);
            }

            BackToMenu.SetAction(OnBackToMenu);

            RenameButton.SetAction(OnRename);
            DeleteButton.SetAction(OnDelete);

            //
            if (SelectedSlotID == -1)
            {
                CreatePlayButton.SetAvailable(false);
                RenameButton.SetAvailable(false);
                DeleteButton.SetAvailable(false);
            }

            // Input Name WindowSection, events     CHECK SetActive(true) reason !
            InputNameWindow.SetActive(true);
            SubmitInputName.onClick.AddListener(OnInputNameSubmit);
            CancelInputName.onClick.AddListener(OnInputNameCancel);
            InputNameWindow.SetActive(false);

            DeletionWindow.SetActive(true);
            SubmitDeletion.onClick.AddListener(OnDeletionSubmit);
            CancelDeletion.onClick.AddListener(OnDeletionCancel);
            DeletionWindow.SetActive(false);

            ScreenCreated = true;
            ResetButtonTexts();
            ResetSelectedView();
        }

        private void Update()
        {
            // DEBUG
            SelectedIDText.text = SelectedSlotID.ToString();
            //

            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    CurrentAction?.Invoke();
                }
            }
        }

        private void ResetSelectedView()
        {
            BackToMenu.SetSound(ESounds.Silence);
            EventSystem.current.SetSelectedGameObject(BackToMenu.gameObject);
            BackToMenu.SelectedLook();
            BackToMenu.SetSound(ESounds.ChooseOption);
        }

        private void ResetButtonTexts()
        {
            Title.text = Localization.Utilitary(EUtilitary.SelectYourGame_Title);
            BackToMenu.SetProperties(TextColors, Localization.Utilitary(EUtilitary.BackToTitle));

            if (SelectedSlotID == -1) CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.CreateButton));
            if (SelectedSlotID > 0 && PlayerStates[SelectedSlotID] == null) CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.CreateButton));
            if (SelectedSlotID > 0 && PlayerStates[SelectedSlotID] != null) CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.PlayButton));

            RenameButton.SetProperties(Localization.Utilitary(EUtilitary.RenameButton));
            DeleteButton.SetProperties(Localization.Utilitary(EUtilitary.DeleteButton));
            
            foreach (var slot in Slots)
            {
                slot.ResetTexts();
            }

            // INPUT WINDOW
            InputNameWindow.SetActive(true);
            InputWindowTitle.text = Localization.Utilitary(EUtilitary.EnterYourName_Title);
            SubmitText.text = Localization.Utilitary(EUtilitary.Submit);
            CancelText.text = Localization.Utilitary(EUtilitary.Cancel);
            InputNameWindow.SetActive(false);

            // DELETION WINDOW
            DeletionWindow.SetActive(true);
            DeletionWindowTitle.text = Localization.Utilitary(EUtilitary.AreYouSure_Title);
            SubmitDeletionText.text = Localization.Utilitary(EUtilitary.Submit);
            CancelDeletionText.text = Localization.Utilitary(EUtilitary.Cancel);
            DeletionWindow.SetActive(false);
        }

        private void SwitchMainLayer(bool flag)
        {
            foreach (var slot in Slots)
            {
                slot.SetInteractable(flag);
            }

            BackToMenu.SetInteractable(flag);

            CreatePlayButton.SetInteractable(flag);
            RenameButton.SetInteractable(flag);
            DeleteButton.SetInteractable(flag);
        }

        private void UpdateButtonActions()
        {
            if (PlayerStates[SelectedSlotID] == null)
            {
                CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.CreateButton));
                CreatePlayButton.SetAction(OnCreateGame);

                RenameButton.SetAvailable(false);
                DeleteButton.SetAvailable(false);
            }

            if (PlayerStates[SelectedSlotID] != null)
            {
                CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.PlayButton));
                CreatePlayButton.SetAction(OnPlayGame);

                RenameButton.SetAvailable(true);
                DeleteButton.SetAvailable(true);
            }
        }

        private void OnSendID(int id)
        {
            SelectedSlotID = id;
            foreach (var slot in Slots)
            {
                // resetting previous submitted slot
                slot.SelectSlot(false);
                slot.DeselectedLook();
            }

            //they work only when slot is submitted
            UpdateButtonActions();

            CreatePlayButton.SetAvailable(true);
        }

        //Input Name Window
        private void OnInputNameSubmit()
        {
            if (NameInput.text == "")
            {
                PlayerStates[SelectedSlotID].Name = NoName;
            }
            else
            {
                PlayerStates[SelectedSlotID].Name = NameInput.text;
            }
            
            ProgressManager.SetState(PlayerStates[SelectedSlotID]);
            Storage.Save(ProgressManager.PlayerState);
            Slots[SelectedSlotID].ResetTexts();

            //visual
            OnInputNameCancel();
        }

        private void OnInputNameCancel()
        {
            InputNameWindow.SetActive(false);
            RenameButton.DeselectedLook();
            ResetSelectedView();
            SwitchMainLayer(true);
            UpdateButtonActions();
        }

        private void OnBackToMenu()
        {
            BackToMenu.SubmittedLook();
            Timer = ClickDelay;
            CurrentAction = BackToMenuClicked;

            AudioManager.PlaySound(SelectSound);
        }

        private void OnRename()
        {
            RenameButton.SubmittedLook();
            SwitchMainLayer(false);
            InputNameWindow.SetActive(true);
            NameInput.text = PlayerStates[SelectedSlotID].Name;
        }

        private void OnDelete()
        {
            DeleteButton.SubmittedLook();
            SwitchMainLayer(false);
            DeletionWindow.SetActive(true);
        }

        private void OnDeletionSubmit()
        {
            Storage.Delete(PlayerStates[SelectedSlotID]);
            ProgressManager.SetState(null);
            PlayerStates[SelectedSlotID] = null;

            Slots[SelectedSlotID].SetProperties(SelectedSlotID, null);
            Slots[SelectedSlotID].ResetTexts();

            UpdateButtonActions();
            SelectedSlotID = -1;

            DeletionWindow.SetActive(false);
            DeleteButton.DeselectedLook();
            ResetSelectedView();
            SwitchMainLayer(true);
        }

        private void OnDeletionCancel()
        {
            DeletionWindow.SetActive(false);
            DeleteButton.DeselectedLook();
            ResetSelectedView();
            SwitchMainLayer(true);
            UpdateButtonActions();
        }

        private void OnCreateGame()
        {
            CreatePlayButton.SubmittedLook();

            PlayerStates[SelectedSlotID] = ProgressManager.CreateState(SelectedSlotID);

            PlayerStates[SelectedSlotID].Name = NoName;

            ProgressManager.SetState(PlayerStates[SelectedSlotID]);
            Storage.Save(ProgressManager.PlayerState);

            //Updating data for selected slot
            Slots[SelectedSlotID].SetProperties(SelectedSlotID, PlayerStates[SelectedSlotID]);
            Slots[SelectedSlotID].ResetTexts();

            CreatePlayButton.DeselectedLook();
            ResetSelectedView();
            UpdateButtonActions();
        }

        private void OnPlayGame()
        {
            CreatePlayButton.SubmittedLook();

            Timer = ClickDelay;
            SwitchMainLayer(false);
            ProgressManager.SetState(PlayerStates[SelectedSlotID]);
            AudioManager.PlaySound(SelectSound);

            UpdateButtonActions();
            CurrentAction = PlayGameClicked;
        }
    }
}