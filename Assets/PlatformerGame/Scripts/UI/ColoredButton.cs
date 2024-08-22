using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{
    public class ColoredButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler
    {
        public event Action Clicked = () => { };

        [SerializeField]
        private Button Button;
        [SerializeField]
        private Text Text;

        private List<Color> Colors;
        private ESounds Sound = ESounds.ChooseOption;

        private IAudioManager AudioManager;

        private void Awake()
        {
            AudioManager = CompositionRoot.GetAudioManager();

            Button = GetComponent<Button>();
            Text = GetComponentInChildren<Text>();

            Button.onClick.AddListener(OnClicked);
        }

        public void SetColors(List<Color> colors)
        {
            Colors = colors;
            Text.color = Colors[1];
        }

        public void SetLabel(string label)
        {
            Text.text = label;
        }

        public void SetSound(ESounds sound)
        {
            Sound = sound;
        }

        public void SetSelected()
        {
            Text.color = Colors[0];
        }

        public void SetDeselected()
        {
            Text.color = Colors[1];
        }

        public void SetSubmitted()
        {
            Text.color = Colors[0];
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (Button.interactable)
            {
                SetSelected();
                AudioManager.PlaySound(Sound);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Button.interactable)
            {
                SetSelected();
                Button.Select();
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (Button.interactable)
            {
                SetDeselected();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (Button.interactable)
            {
                SetSubmitted();
            }
        }

        private void OnClicked()
        {
            Clicked();
        }
    }
}