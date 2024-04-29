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
        private ETexts Label;

        [SerializeField]
        private Outline Outline;



        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D Trigger;

        private bool Inside;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            Trigger = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            Text.text = Localization.Text(Label);
            Outline.enabled = false;
        }

        private void Update()
        {
            if (Trigger.bounds.Contains(Player.Position) == true && Inside == false)
            {
                Inside = true;
                Outline.enabled = true;
            }

            if (Trigger.bounds.Contains(Player.Position) == false && Inside == true)
            {
                Inside = false;
                Outline.enabled = false;
            }
        }
    }
}