using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class Entrance : MonoBehaviour
    {
        private ILocalization Localization;
        private IPlayer Player;

        [SerializeField]
        private List<GameObject> NewLocation;

        [SerializeField]
        private List<GameObject> OldLocation;

        [SerializeField]
        ETexts Label;

        [SerializeField]
        Text HelpText;

        private float Timer;
        private float BlinkTime = 0.5f;
        private bool InsideTrigger;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
        }

        private void OnEnable()
        {
            HelpText.text = Localization.Text(Label);
            Timer = BlinkTime;
            HelpText.enabled = false;
        }

        private void Update()
        {
            if (InsideTrigger)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    Timer = BlinkTime;
                    HelpText.enabled = !HelpText.enabled;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                InsideTrigger = true;
                Timer = BlinkTime;
                HelpText.enabled = true;
                // subscribe OnLocationEnter
                Player.Interaction += OnLocationEnter;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                InsideTrigger = false;
                HelpText.enabled = false;
                // unsubscribe Player's action..
                Player.Interaction -= OnLocationEnter;
            }
        }

        private void OnDisable()
        {
            // unsubscribe Player's action..
            InsideTrigger = false;
            HelpText.enabled = false;
            if (Player != null)
            {
                Player.Interaction -= OnLocationEnter;
            }
        }

        private void OnLocationEnter()
        {
            for (int i = 0; i < OldLocation.Count; i++)
            {
                OldLocation[i].SetActive(false);
            }

            for (int i = 0; i < NewLocation.Count; i++)
            {
                NewLocation[i].SetActive(true);
            }
        }

    }
}