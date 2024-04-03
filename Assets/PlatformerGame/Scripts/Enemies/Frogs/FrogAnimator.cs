using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FrogAnimator : BlinkAnimator
    {
        [SerializeField]
        private List<Sprite> Idle;

        [SerializeField]
        private List<Sprite> Attack;

        [SerializeField]
        private List<Sprite> JumpRise;

        [SerializeField]
        private List<Sprite> JumpFall;

        [SerializeField]
        private List<Sprite> Death;

        [SerializeField]
        private float AnimationDelay;

        private float BackupAnimationPeriod; //Megafrog ?

        private void OnEnable()
        {
            //CurrentState = Begin;
        }

        // for Megafrog-boss "Defeat" behaviour
        public void StartEndlessBlinking()
        {
            BlinkTimer = BlinkDelay;
            EffectDuration = 30f;

            StartBlinking();
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

        public float PlayAttack()
        {
            CurrentAnimation = Attack;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayJumpRise()
        {
            CurrentAnimation = JumpRise;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayJumpFall()
        {
            CurrentAnimation = JumpFall;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayDeath()
        {
            CurrentAnimation = Death;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }


        public void SetNewAnimationPeriod(float period)
        {
            BackupAnimationPeriod = AnimationDelay;
            AnimationDelay = period;
        }

        public void RestoreAnimationPeriod()
        {
            if (BackupAnimationPeriod != 0)
            {
                AnimationDelay = BackupAnimationPeriod;
            }
        }

    }
}