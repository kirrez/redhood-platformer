using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class NavigationButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler
    {
        private Button Button;
        private Text Text;

        private List<Color> Colors;
        private ESounds Sound = ESounds.ChooseOption;

        private IAudioManager AudioManager;

        private void Awake()
        {
            AudioManager = CompositionRoot.GetAudioManager();

            Button = GetComponent<Button>();
            Text = GetComponentInChildren<Text>();
        }

        public void SetProperties(List<Color> colors, string label)
        {
            Colors = colors;
            Text.color = Colors[1];
            Text.text = label;
        }

        public void SetAction(UnityAction action)
        {
            Button.onClick.AddListener(action);
        }

        public void SetInteractable(bool flag)
        {
            Button.interactable = flag;
        }

        public void SetSound(ESounds sound)
        {
            Sound = sound;
        }

        public void SelectedLook()
        {
            Text.color = Colors[0];
        }

        public void DeselectedLook()
        {
            Text.color = Colors[1];
        }

        public void SubmittedLook()
        {
            Text.color = Colors[0];
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (Button.interactable)
            {
                SelectedLook();
                AudioManager.PlaySound(Sound);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Button.interactable)
            {
                SelectedLook();
                Button.Select();
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (Button.interactable)
            {
                DeselectedLook();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (Button.interactable)
            {
                SubmittedLook();
            }
        }
    }
}