using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{
    public class SlotWidget : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerEnterHandler
    {
        public event Action<SlotWidget> Clicked = widget => { };
        public event Action<SlotWidget> Hovered = widget => { };

        public Text NameLabel;
        public Text NameValue;

        public Text DateValue;
        public Text TimeValue;

        public Text ModeLabel;
        public Text ModeValue;

        public Text TimePlayedLabel;
        public Text TimePlayedValue;

        public Text NoDataLabel;
        public GameObject NoDataPanel;

        public Image Background;

        public Sprite DefaultBorder;
        public Sprite SelectedBorder;
        public Sprite HoveredBorder;

        public Button Button;

        private ILocalization Localization;
        private IAudioManager AudioManager;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();

            Button.onClick.AddListener(OnClicked);
        }

        private void OnEnable()
        {
            UpdateLocalization();
        }

        public void SetName(string text)
        {
            NameValue.text = text;
        }

        public void SetDateTime(DateTime newDate, TimeSpan newTime)
        {
            var date = newDate.ToString("yyyy.MM.dd");

            var hours = newTime.Hours;
            var minutes = newTime.Minutes;

            DateValue.text = date;
            //TimeValue.text = string.Format("{0} : {1}", hours, minutes);
            TimeValue.text = hours.ToString("00") + " : " + minutes.ToString("00");
        }

        public void SetPlayedTime(TimeSpan value)
        {
            var hours = value.Hours;
            var minutes = value.Minutes;

            //TimePlayedValue.text = string.Format("{0}h {1}min", hours, minutes);
            TimePlayedValue.text = hours.ToString("00") + "h " + minutes.ToString("00") + "min";
        }

        public void SetDifficulty(int mode)
        {
            if (mode == 0)
            {
                ModeValue.text = Localization.Utilitary(EUtilitary.EasyMode);
            }
            if (mode == 1)
            {
                ModeValue.text = Localization.Utilitary(EUtilitary.NormalMode);
            }
            // hard and whatever we'll add later ))
        }

        public void SetHoveredBorder()
        {
            Background.sprite = HoveredBorder;
        }

        public void SetSelectedBorder()
        {
            Background.sprite = SelectedBorder;
        }

        public void SetDefaultBorder()
        {
            Background.sprite = DefaultBorder;
        }

        public void ShowEmptyMessage()
        {
            NoDataPanel.gameObject.SetActive(true);
        }

        public void HideEmptyMessage()
        {
            NoDataPanel.gameObject.SetActive(false);
        }

        public void UpdateLocalization()
        {
            var gameNameLabel = Localization.Utilitary(EUtilitary.GameNameLabel);
            var difficultyModeLabel = Localization.Utilitary(EUtilitary.DifficultyModeLabel);
            var timePlayedLabel = Localization.Utilitary(EUtilitary.TimePlayed);
            var noDataText = Localization.Utilitary(EUtilitary.NoData);

            NoDataLabel.text = noDataText;
            NameLabel.text = gameNameLabel;
            ModeLabel.text = difficultyModeLabel;
            TimePlayedLabel.text = timePlayedLabel;
        }

        //Pointer Events

        public void OnPointerEnter(PointerEventData eventData)
        {
            Hovered(this);
        }

        public void OnSelect(BaseEventData eventData)
        {
            Hovered(this);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Clicked(this);
        }

        private void OnClicked()
        {
            Clicked(this);
        }
    }
}