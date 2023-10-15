using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DestructionTimer : MonoBehaviour
    {
        [SerializeField]
        private float Duration;

        private float Timer;

        private void OnEnable()
        {
            Timer = Duration;
        }

        private void FixedUpdate()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0) return;

            gameObject.SetActive(false);
        }
    }
}