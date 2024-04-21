using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class RapidDamage : MonoBehaviour
    {
        [SerializeField]
        private bool IsRapid;

        private Collider2D Collider;

        private float Timer;
        private float Delay = 0.1f;

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (IsRapid == false) return;

            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Timer = Delay;
                Collider.enabled = !Collider.enabled;
            }
        }
    }
}