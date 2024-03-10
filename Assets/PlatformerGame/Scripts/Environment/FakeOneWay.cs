using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FakeOneWay : MonoBehaviour
    {
        [SerializeField]
        private GameObject Image;

        [SerializeField]
        private Collider2D HideTrigger;

        [SerializeField]
        private Collider2D ShowTrigger;

        private IPlayer Player;
        private IAudioManager AudioManager;

        private float Timer;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            AudioManager = CompositionRoot.GetAudioManager();
        }

        private void OnEnable()
        {
            Image.SetActive(true);
            CurrentState = StateOutsideHide;
        }

        private void Update()
        {
            CurrentState();
        }

        private void StateOutsideHide()
        {
            if (HideTrigger.bounds.Contains(Player.Position))
            {
                Image.SetActive(false);
                CurrentState = StateInsideShow;
                AudioManager.PlaySound(ESounds.BulletShot1);
            }
        }

        private void StateInsideShow()
        {
            if(ShowTrigger.bounds.Contains(Player.Position) == false)
            {
                Timer = 2f;
                CurrentState = StateRest;
            }
        }

        private void StateRest()
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Image.SetActive(true);
                CurrentState = StateOutsideHide;
            }
        }
    }
}