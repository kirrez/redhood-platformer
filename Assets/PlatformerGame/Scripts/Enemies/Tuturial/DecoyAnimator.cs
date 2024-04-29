using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DecoyAnimator : BlinkAnimator
    {
        [SerializeField]
        private List<Sprite> Idle;

        public float PlayIdle()
        {
            CurrentAnimation = Idle;
            Renderer.sprite = CurrentAnimation[0];

            Delay = 0.5f;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }


    }
}