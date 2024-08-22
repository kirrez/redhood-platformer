using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class PlayScreenView : BaseView, IPlayScreenView
    {
        private const string DefaultName = "NoName";

        public event Action<int> SlotSelected = slotIndex => { };

        public event Action<int> PlayClicked = slotIndex => { };
        public event Action<int> CreateClicked = slotIndex => { };
        public event Action<int> RenameClicked = slotIndex => { };
        public event Action<int> DeleteClicked = slotIndex => { };

        public event Action DeleteCanceled = () => { };
        public event Action RenameCanceled = () => { };
        public event Action<int> DeleteSubmitted = slotIndex => { };
        public event Action<int, string> RenameSubmitted = (slotIndex, name) => { };

        public event Action BackClicked = () => { };

        public Text Title;

        [Header("Buttons")]
        [Space(20)]
        public ColoredButton BackButton;
        public SubmitButton CreateButton;
        public SubmitButton PlayButton;
        public SubmitButton RenameButton;
        public SubmitButton DeleteButton;

        [Header("Rename Window")]
        [Space(20)]
        public GameObject RenamePopup;
        public Text RenamePopupTitle;
        public InputField RenameInputField;
        public Text RenameSubmitText;
        public Text RenameCancelText;
        public Button RenameSubmitButton;
        public Button RenameCancelButton;

        [Header("Delete Window")]
        [Space(20)]
        public GameObject DeletePopup;
        public Text DeletePopupTitle;
        public Text DeleteSubmitText;
        public Text DeleteCancelText;
        public Button DeleteSubmitButton;
        public Button DeleteCancelButton;

        public List<Color> TextColors;
        public List<SlotWidget> Slots;

        private int SelectedSlotIndex = -1;

        private ILocalization Localization;
        private IAudioManager AudioManager;
        
        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();

            BackButton.Clicked += OnBackClicked;
            CreateButton.Clicked += OnCreateClicked;
            PlayButton.Clicked += OnPlayClicked;
            RenameButton.Clicked += OnRenameClicked;
            DeleteButton.Clicked += OnDeleteClicked;

            RenameCancelButton.onClick.AddListener(OnRenameCanceled);
            RenameSubmitButton.onClick.AddListener(OnRenameSubmitted);
            DeleteSubmitButton.onClick.AddListener(OnDeleteSubmitted);
            DeleteCancelButton.onClick.AddListener(OnDeleteCanceled);

            foreach (var slot in Slots)
            {
                slot.Clicked += OnSlotClicked;
                slot.Hovered += OnSlotHovered;
            }

            ShowCreateButton();
        }

        private void OnEnable()
        {
            ResetButtonTexts();
        }

        public void ShowRenamePopup(string name)
        {
            RenameInputField.text = name;
            RenamePopup.gameObject.SetActive(true);
        }

        public void HideRenamePopup()
        {
            RenamePopup.gameObject.SetActive(false);
        }

        public void ShowDeletePopup(string name)
        {
            DeletePopupTitle.text = name;
            DeletePopup.gameObject.SetActive(true);
        }

        public void HideDeletePopup()
        {
            DeletePopup.gameObject.SetActive(false);
        }

        public void ShowPlayButton()
        {
            PlayButton.gameObject.SetActive(true);
            CreateButton.gameObject.SetActive(false);
        }

        public void ShowCreateButton()
        {
            PlayButton.gameObject.SetActive(false);
            CreateButton.gameObject.SetActive(true);
        }

        public void FillSlot(int index)
        {
            var slot = Slots[index];
            slot.HideEmptyMessage();
        }

        public void EmptySlot(int index)
        {
            var slot = Slots[index];
            slot.ShowEmptyMessage();
        }

        public void SelectSlot(int index)
        {
            SelectedSlotIndex = index;

            var widget = Slots[index];

            foreach (var slot in Slots)
            {
                if (slot == widget)
                {
                    slot.SetSelectedBorder();
                }
                else
                {
                    slot.SetDefaultBorder();
                }
            }

            SlotSelected(index);
        }

        public void SetSlotName(int index, string name)
        {
            var slot = Slots[index];
            slot.SetName(name);
        }

        public void SetSlotDate(int index, DateTime date)
        {
            var slot = Slots[index];
            slot.SetDate(date);
        }

        public void SetSlotPlayedTime(int index, TimeSpan time)
        {
            var slot = Slots[index];
            slot.SetPlayedTime(time);
        }

        private void ResetButtonTexts()
        {
            var title = Localization.Utilitary(EUtilitary.SelectYourGame_Title);
            var backButtonLabel = Localization.Utilitary(EUtilitary.BackToTitle);
            var renameButtonLabel = Localization.Utilitary(EUtilitary.RenameButton);
            var deleteButtonLabel = Localization.Utilitary(EUtilitary.DeleteButton);
            var playButtonLabel = Localization.Utilitary(EUtilitary.PlayButton);
            var createButtonLabel = Localization.Utilitary(EUtilitary.CreateButton);

            Title.text = title;

            BackButton.SetColors(TextColors);
            BackButton.SetLabel(backButtonLabel);

            //if (SelectedSlotID == -1) CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.CreateButton));
            //if (SelectedSlotID > 0 && PlayerStates[SelectedSlotID] == null) CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.CreateButton));
            //if (SelectedSlotID > 0 && PlayerStates[SelectedSlotID] != null) CreatePlayButton.SetProperties(Localization.Utilitary(EUtilitary.PlayButton));

            PlayButton.SetProperties(playButtonLabel);
            CreateButton.SetProperties(createButtonLabel);
            RenameButton.SetProperties(renameButtonLabel);
            DeleteButton.SetProperties(deleteButtonLabel);

            //foreach (var slot in Slots)
            //{
            //    slot.UpdateLocalization();
            //}

            // INPUT WINDOW
            RenamePopup.SetActive(true);
            RenamePopupTitle.text = Localization.Utilitary(EUtilitary.EnterYourName_Title);
            RenameSubmitText.text = Localization.Utilitary(EUtilitary.Submit);
            RenameCancelText.text = Localization.Utilitary(EUtilitary.Cancel);
            RenamePopup.SetActive(false);

            // DELETION WINDOW
            DeletePopup.SetActive(true);
            DeletePopupTitle.text = Localization.Utilitary(EUtilitary.AreYouSure_Title);
            DeleteSubmitText.text = Localization.Utilitary(EUtilitary.Submit);
            DeleteCancelText.text = Localization.Utilitary(EUtilitary.Cancel);
            DeletePopup.SetActive(false);
        }

        private void OnDeleteSubmitted()
        {
            DeleteSubmitted(SelectedSlotIndex);
        }

        private void OnRenameSubmitted()
        {
            RenameSubmitted(SelectedSlotIndex, RenameInputField.text);
        }

        private void OnRenameCanceled()
        {
            RenameCanceled();
        }

        private void OnDeleteClicked()
        {
            DeleteClicked(SelectedSlotIndex);
        }

        private void OnDeleteCanceled()
        {
            DeleteCanceled();
        }

        private void OnRenameClicked()
        {
            RenameClicked(SelectedSlotIndex);
        }

        private void OnPlayClicked()
        {
            PlayClicked(SelectedSlotIndex);
        }

        private void OnCreateClicked()
        {
            CreateClicked(SelectedSlotIndex);
        }

        private void OnSlotClicked(SlotWidget widget)
        {
            var index = Slots.IndexOf(widget);

            SelectSlot(index);
        }

        private void OnSlotHovered(SlotWidget widget)
        {
            var selectedWidget = SelectedSlotIndex == -1 ? null : Slots[SelectedSlotIndex];

            foreach (var slot in Slots)
            {
                if (slot == selectedWidget)
                {
                    continue;
                }

                if (slot == widget)
                {
                    slot.SetHoveredBorder();
                }
                else
                {
                    slot.SetDefaultBorder();
                }
            }
        }

        private void OnBackClicked()
        {
            BackClicked();
        }
    }
}