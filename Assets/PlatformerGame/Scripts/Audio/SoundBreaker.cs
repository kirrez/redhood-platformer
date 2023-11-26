using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SoundBreaker : MonoBehaviour
    {
        private AudioSource source;
        private float Timer;
        private bool IsWorking;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void StartCountDown()
        {
            Timer = source.clip.length;
            IsWorking = true;
        }

        private void Update()
        {
            if (!IsWorking) return;

            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                IsWorking = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}