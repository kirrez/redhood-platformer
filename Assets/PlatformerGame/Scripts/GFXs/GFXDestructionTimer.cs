using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class GFXDestructionTimer : MonoBehaviour
    {
        [SerializeField]
        private float MinDuration = -1f;

        [SerializeField]
        private float Duration;

        private float Timer;

        private void OnEnable()
        {
            if (MinDuration >= 0)
            {
                Timer = Random.Range(MinDuration, Duration);
            }
            else
            {
                Timer = Duration;
            }
        }

        private void FixedUpdate()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0) return;

            var instance = GetComponent<BaseGFX>();
            if (instance != null)
            {
                instance.Disappear?.Invoke();
            }
        }
    }
}