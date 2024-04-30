using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class TextOutliner : MonoBehaviour
    {
        [SerializeField]
        private Text Text;

        [SerializeField]
        private ETutorialTexts Name;

        [SerializeField]
        private Outline Outline;

        private IAudioManager AudioManager;
        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D Trigger;

        private bool Inside;

        private void Awake()
        {
            AudioManager = CompositionRoot.GetAudioManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            Trigger = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            Text.text = Localization.Tutorial(Name);
            Outline.enabled = false;
        }

        private void Update()
        {
            if (Trigger.bounds.Contains(Player.Position) == true && Inside == false)
            {
                Inside = true;
                Outline.enabled = true;
                AudioManager.PlaySound(ESounds.BatStart);
            }

            if (Trigger.bounds.Contains(Player.Position) == false && Inside == true)
            {
                Inside = false;
                Outline.enabled = false;
            }
        }
    }
}