using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BearCubAnimator : BlinkAnimator
    {
        [SerializeField]
        private List<Sprite> Walk;

        [SerializeField]
        private List<Sprite> Jump;

        [SerializeField]
        private List<Sprite> Dying;

        [SerializeField]
        private List<Sprite> Appear;

        [SerializeField]
        private float WalkDelay;

        [SerializeField]
        private float DyingDelay;

        [SerializeField]
        private float AppearBlinkDelay;

        public float PlayAppear()
        {
            CurrentAnimation = Appear;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AppearBlinkDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayWalk()
        {
            CurrentAnimation = Walk;
            Renderer.sprite = CurrentAnimation[0];

            Delay = WalkDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count -1) * Delay;
        }

        public float PlayDying()
        {
            CurrentAnimation = Dying;
            Renderer.sprite = CurrentAnimation[0];

            Delay = DyingDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayJump()
        {
            CurrentAnimation = Jump;
            Renderer.sprite = CurrentAnimation[0];

            Delay = 1f;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }
    }
}
