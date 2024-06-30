using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Platformer
{
    public class SubmitButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler
    {
        private Button Button;
        private Image Image;
        private Text Text;

        private bool Available;

        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private List<Color> TextColors;

        private void Awake()
        {
            Button = GetComponent<Button>();
            Image = GetComponent<Image>();
            Text = GetComponentInChildren<Text>();
        }

        public void SetProperties(string label)
        {
            Text.text = label;
        }

        public void SetAction(UnityAction action)
        {
            // some buttons can change their function
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(action);
        }

        public void SetInteractable(bool flag)
        {
            Button.interactable = flag;
        }

        public void DeselectedLook()
        {
            Image.sprite = Sprites[0];
            Text.color = TextColors[0];
        }

        public void SelectedLook()
        {
            Image.sprite = Sprites[1];
            Text.color = TextColors[1];
        }

        public void SubmittedLook()
        {
            Image.sprite = Sprites[2];
            Text.color = TextColors[1];
        }

        public void InactiveLook()
        {
            Image.sprite = Sprites[3];
            Text.color = TextColors[2];
        }
        
        //
        public bool IsAvailable()
        {
            return Available;
        }

        public void SetAvailable(bool flag)
        {
            if  (flag == true)
            {
                Available = true;
                DeselectedLook();
                SetInteractable(true);
            }

            if (flag == false)
            {
                Available = false;
                InactiveLook();
                SetInteractable(false);
            }
        }


        public void OnSelect(BaseEventData eventData)
        {
            if (Button.interactable == false) return;
            if (Available == false) return;
            
            SelectedLook();
            //AudioManager.PlaySound(Sound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Button.interactable == false) return;
            if (Available == false) return;

            SelectedLook();
            Button.Select();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (Button.interactable == false) return;
            if (Available == false) return;

            DeselectedLook();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (Button.interactable == false) return;
            if (Available == false) return;

            SubmittedLook();
        }
    }
}