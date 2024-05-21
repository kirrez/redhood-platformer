using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CursedFlame : CursedObstacle
    {
        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private GameObject ActiveFlame;

        [SerializeField]
        private GameObject InactiveFlame;

        [SerializeField]
        private bool IsActive;

        [SerializeField]
        private float RegenerateDelay = 1f;

        [SerializeField]
        private float RegenerateBlinkDelay = 1f;

        [SerializeField]
        private float BlinkDelay = 0.1f;

        private float Timer;
        private float BlinkTimer;
        private bool IsOpaque;

        protected delegate void State();
        protected State CurrentState = () => { };

        private void Awake()
        {
            Dispel = OnDispel;
        }

        private void OnEnable()
        {
            CurrentState = StateSetup;
        }

        private void Update()
        {
            CurrentState();
        }

        public void Activate()
        {
            IsActive = true;
            CurrentState = StateSetup;
        }

        private void StateSetup()
        {
            if (IsActive == true)
            {
                Body.gameObject.SetActive(true);
                ActiveFlame.SetActive(true);
                InactiveFlame.SetActive(false);
                CurrentState = StateBurning;
            }

            if (IsActive == false)
            {
                Body.gameObject.SetActive(false);
                ActiveFlame.SetActive(false);
                InactiveFlame.SetActive(true);
                CurrentState = () => { };
            }
        }

        private void StateBurning()
        {
            //doing nothing
        }

        private void StateExtinguished()
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Timer = RegenerateBlinkDelay;
                BlinkTimer = BlinkDelay;
                IsOpaque = true;

                CurrentState = StateBlinking;
            }
        }

        private void StateBlinking()
        {
            Color color = Color.white;
            var flame = InactiveFlame.GetComponent<SpriteRenderer>();

            Timer -= Time.deltaTime;
            BlinkTimer -= Time.deltaTime;

            if (BlinkTimer <= 0)
            {
                IsOpaque = !IsOpaque;
                BlinkTimer = BlinkDelay;

                if (IsOpaque == true)
                {
                    color.a = 0.15f;
                    flame.color = color;
                }

                if (IsOpaque == false)
                {
                    color.a = 1f;
                    flame.color = color;
                }
            }

            if (Timer <= 0)
            {
                color.a = 1f;
                flame.color = color;
                Body.gameObject.SetActive(true);
                ActiveFlame.SetActive(true);
                InactiveFlame.SetActive(false);

                CurrentState = StateBurning;
            }
        }

        private void OnDispel()
        {
            Timer = RegenerateDelay;

            Body.gameObject.SetActive(false);
            ActiveFlame.SetActive(false);
            InactiveFlame.SetActive(true);

            CurrentState = StateExtinguished;
        }
    }
}