using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class MessageCanvas : MonoBehaviour
    {
        [SerializeField]
        private Text Text;

        private IDynamicsContainer DynamicsContainer;

        private bool IsBlinking;
        private float Timer;
        private float Period = 0.5f;

        private delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
        }

        private void OnEnable()
        {
            DynamicsContainer.AddMain(this.gameObject);

            if (IsBlinking)
            {
                CurrentState = StartVisible;
            }
        }

        private void OnDisable()
        {
            IsBlinking = false;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void StartVisible()
        {
            Timer = Period;
            Text.enabled = true;
            CurrentState = WaitVisible;
        }

        private void WaitVisible()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Text.enabled = false;
                Timer = Period;
                CurrentState = WaitInvisible;
            }
        }

        private void WaitInvisible()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                CurrentState = StartVisible;
            }
        }

        public void StopBlinking()
        {
            Text.enabled = true;
            CurrentState = null;
            CurrentState = () => { };
        }

        public void SetBlinking(bool blinking, float period)
        {
            IsBlinking = blinking;
            Period = period;

            if (IsBlinking)
            {
                CurrentState = StartVisible;
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetMessage(string message)
        {
            Text.text = message;
        }
    }
}