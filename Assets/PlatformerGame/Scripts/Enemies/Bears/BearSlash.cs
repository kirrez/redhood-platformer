using System.Collections;
using System.Collections.Generic;
using Platformer.ScriptedAnimator;
using UnityEngine;

namespace Platformer
{
    public class BearSlash : MonoBehaviour
    {
        private SimpleAnimation Animation;
        private Collider2D Collider;

        private float Timer;

        public void SetHitDirection(float direction)
        {
            GetComponent<DamageDealer>().SetHitDirection(direction);
            if (direction == 1)
            {
                Animation.SetFlipX(false);
            }

            if (direction == -1)
            {
                Animation.SetFlipX(true);
            }
        }

        private void Awake()
        {
            Animation = GetComponent<SimpleAnimation>();
            Collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            Collider.enabled = true;
            Timer = 0.6f; // magic number, period of collider's activity
        }

        private void FixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Collider.enabled = false;
        }
    }
}