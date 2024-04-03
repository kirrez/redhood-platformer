using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MummyAnimator : UndeadAnimator
    {
        [SerializeField]
        private List<Sprite> Walking;

        [SerializeField]
        private float AnimationDelay;

        public float PlayWalking()
        {
            CurrentAnimation = Walking;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }
    }
}