using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BearAnimator : BlinkAnimator
    {
        [SerializeField]
        private List<Sprite> Idle;

        [SerializeField]
        private List<Sprite> Walk;

        [SerializeField]
        private List<Sprite> Attack;

        [SerializeField]
        private List<Sprite> Dying;

        [SerializeField]
        private List<Sprite> Appear;

        [SerializeField]
        private float AnimationDelay;

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

        public float PlayIdle()
        {
            CurrentAnimation = Idle;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayWalk()
        {
            CurrentAnimation = Walk;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayAttack()
        {
            CurrentAnimation = Attack;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayDying()
        {
            CurrentAnimation = Dying;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }
    }
}