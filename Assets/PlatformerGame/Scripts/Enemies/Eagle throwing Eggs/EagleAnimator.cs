using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EagleAnimator : BlinkAnimator
    {
        [SerializeField]
        private List<Sprite> Idle;

        [SerializeField]
        private List<Sprite> Dying;

        [SerializeField]
        private List<Sprite> Spawning;

        [SerializeField]
        private float IdleDelay = 0.25f;

        [SerializeField]
        private float DyingDelay = 0.25f;

        [SerializeField]
        private float SpawningDelay = 0.25f;

        public float PlayIdle()
        {
            CurrentAnimation = Idle;
            Delay = IdleDelay;
            Timer = Delay;
            Index = 0;

            Renderer.sprite = CurrentAnimation[Index];

            return CurrentAnimation.Count * Delay;
        }

        public float PlayDying()
        {
            CurrentAnimation = Dying;
            Delay = DyingDelay;
            Timer = Delay;
            Index = 0;

            Renderer.sprite = CurrentAnimation[Index];

            return CurrentAnimation.Count * Delay;
        }

        public float PlaySpawning()
        {
            CurrentAnimation = Spawning;
            Delay = SpawningDelay;
            Timer = Delay;
            Index = 0;

            Renderer.sprite = CurrentAnimation[Index];

            return CurrentAnimation.Count * Delay;
        }
    }
}