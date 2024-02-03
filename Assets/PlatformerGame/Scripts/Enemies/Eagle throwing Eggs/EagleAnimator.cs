using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EagleAnimator : MonoBehaviour
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

        private List<Sprite> CurrentAnimation;
        private SpriteRenderer Renderer;

        private float Timer;
        private float Delay;
        private int Index;

        private delegate void State();
        State CurrentState = () => { };

        private void FixedUpdate()
        {
            CurrentState();
        }

        public void Initiate(SpriteRenderer renderer)
        {
            Renderer = renderer;
        }

        public float PlayIdle()
        {
            CurrentAnimation = Idle;
            Delay = IdleDelay;
            Timer = Delay;
            Index = 0;

            Renderer.sprite = CurrentAnimation[Index];

            CurrentState = PlayAnimation;
            return CurrentAnimation.Count * Delay;
        }

        public float PlayDying()
        {
            CurrentAnimation = Dying;
            Delay = DyingDelay;
            Timer = Delay;
            Index = 0;

            Renderer.sprite = CurrentAnimation[Index];

            CurrentState = PlayAnimation;
            return CurrentAnimation.Count * Delay;
        }

        public float PlaySpawning()
        {
            CurrentAnimation = Spawning;
            Delay = SpawningDelay;
            Timer = Delay;
            Index = 0;

            Renderer.sprite = CurrentAnimation[Index];

            CurrentState = PlayAnimation;
            return CurrentAnimation.Count * Delay;
        }

        public void Stop()
        {
            CurrentState = () => { };
        }

        private void PlayAnimation()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = Delay;

                if (Index == CurrentAnimation.Count - 1)
                {
                    Index = 0;
                }
                else if (Index < CurrentAnimation.Count - 1)
                {
                    Index++;
                }

                Renderer.sprite = CurrentAnimation[Index];
            }
        }
    }
}