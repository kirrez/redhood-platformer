using System.Collections.Generic;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class Entrance : MonoBehaviour
    {
        private IDynamicsContainer Dynamics;
        private ILocalization Localization;
        private IPlayer Player;

        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int ConfinerIndex;

        [SerializeField]
        [Range(0, 3)]
        private int LabelIndex;

        [SerializeField]
        private Text HelpText;


        private ETexts Label;
        private float Timer;
        private float BlinkTime = 0.5f;
        private bool InsideTrigger;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            Dynamics = CompositionRoot.GetDynamicsContainer();
        }

        private void OnEnable()
        {
            var targetValue = (int)ETexts.Enter + LabelIndex;
            Label = (ETexts)targetValue;
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
            var navigation = CompositionRoot.GetNavigation();
            Dynamics.DeactivateAll();
            navigation.SetLocation(LocationIndex, SpawnPointIndex, ConfinerIndex);

            var game = CompositionRoot.GetGame();
            game.FadeScreen.DelayBefore(Color.black, 0.5f);
            game.FadeScreen.FadeOut(Color.black, 1f);
        }

    }
}