using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{
    public class GameSlot : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler
    {
        [SerializeField]
        private Text NameLabel;

        [SerializeField]
        private Text NameValue;

        [SerializeField]
        private Text DateValue;

        [SerializeField]
        private Text TimeValue;

        [SerializeField]
        private Text ModeLabel;

        [SerializeField]
        private Text ModeValue;

        [SerializeField]
        private Text TimePlayedLabel;

        [SerializeField]
        private Text TimePlayedValue;

        [SerializeField]
        private Text NoDataLabel;

        [SerializeField]
        private GameObject NoDataPanel;

        [SerializeField]
        private Image Background;

        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private Button SlotButton;

        private ESounds Sound = ESounds.ChooseOption;

        private ILocalization Localization;
        private IAudioManager AudioManager;

        private bool IsSlotSelected;
        private int SlotID;
        private IPlayerState PlayerState;

        public delegate void IDSender(int id);
        public event IDSender SendID;


        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
        }

        private void OnEnable()
        {
            
        }

        public void SetProperties(int id, IPlayerState playerState)
        {
            SlotID = id;
            PlayerState = playerState;
        }

        public void SelectSlot(bool select)
        {
            IsSlotSelected = select;
        }

        //should i use this?
        public void SetAction(UnityAction action)
        {
            // some buttons can change their function
            SlotButton.onClick.RemoveAllListeners();
            SlotButton.onClick.AddListener(action);
        }

        public void SetInteractable(bool flag)
        {
            SlotButton.interactable = flag;
        }

        public void SetSound(ESounds sound)
        {
            Sound = sound;
        }

        public void ResetTexts()
        {
            // Labels
            NameLabel.text = Localization.Utilitary(EUtilitary.GameNameLabel);
            ModeLabel.text = Localization.Utilitary(EUtilitary.DifficultyModeLabel);
            TimePlayedLabel.text = Localization.Utilitary(EUtilitary.TimePlayed);

            NoDataPanel.SetActive(true);
            NoDataLabel.text = Localization.Utilitary(EUtilitary.NoData);

            if (PlayerState != null)
            {
                NoDataPanel.SetActive(false);

                NameValue.text = PlayerState.Name;

                //must add 'NORMAL' and 'HARD' in future 
                if (PlayerState.DifficultyMode == 0)
                {
                    ModeValue.text = Localization.Utilitary(EUtilitary.EasyMode);
                }

                DateValue.text = $"{PlayerState.DateYear}.{PlayerState.DateMonth}.{PlayerState.DateDay}";

                TimeValue.text = $"{PlayerState.TimeHours}:{PlayerState.TimeMinutes}";

                TimePlayedValue.text = $"{PlayerState.ElapsedHours}h {PlayerState.ElapsedMinutes}min";
            }

            if (PlayerState == null)
            {
                NoDataPanel.SetActive(true);
                //Debug.Log("EMPTY SLOT!");
            }
        }

        public void DeselectedLook()
        {
            Background.sprite = Sprites[0];
        }

        public void SelectedLook()
        {
            Background.sprite = Sprites[1];
        }

        public void SubmittedLook()
        {
            Background.sprite = Sprites[2];
        }

        public void PerformSubmit()
        {
            if (SlotButton.interactable == false) return;

            SendID?.Invoke(SlotID);

            SelectSlot(true);
            SubmittedLook();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (SlotButton.interactable == false) return;

            if (IsSlotSelected == false)
            {
                SelectedLook();
                AudioManager.PlaySound(Sound);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (SlotButton.interactable == false) return;

            if (IsSlotSelected == false)
            {
                SelectedLook();
                SlotButton.Select();
                AudioManager.PlaySound(Sound);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (SlotButton.interactable == false) return;

            if (IsSlotSelected == false)
            {
                DeselectedLook();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PerformSubmit();
        }
    }
}