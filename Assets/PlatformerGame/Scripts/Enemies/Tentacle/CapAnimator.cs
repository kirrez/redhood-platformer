using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer
{
    public class CapAnimator : BlinkAnimator
    {
        [SerializeField]
        private List<Sprite> IdleAnimation;

        [SerializeField]
        private List<Sprite> ClosingAnimation; // after tentacle's destruction

        [SerializeField]
        private List<Sprite> OpeningAnimation; // before tentacle respawn

        [SerializeField]
        private List<Sprite> HybernatingAnimation; // while tentacle dead


        [SerializeField]
        private float IdleDelay = 0.15f;

        [SerializeField]
        private float ClosingDelay = 0.15f;

        [SerializeField]
        private float OpeningDelay = 0.15f;

        [SerializeField]
        private float HybernatingDelay = 0.15f;


        public float PlayIdle()
        {
            CurrentAnimation = IdleAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = IdleDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayClosing()
        {
            CurrentAnimation = ClosingAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = ClosingDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayOpening()
        {
            CurrentAnimation = OpeningAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = OpeningDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayHybernating()
        {
            CurrentAnimation = HybernatingAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = HybernatingDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }
    }
}