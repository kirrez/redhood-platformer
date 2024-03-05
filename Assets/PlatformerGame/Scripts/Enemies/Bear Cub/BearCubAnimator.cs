using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BearCubAnimator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer NormalRenderer;

        [SerializeField]
        private SpriteRenderer BlinkRenderer;

        [SerializeField]
        private SpriteMask Mask;

        [SerializeField]
        private List<Sprite> Walk;

        [SerializeField]
        private List<Sprite> Jump;

        [SerializeField]
        private List<Sprite> Dying;

        [SerializeField]
        private float WalkDelay;

        [SerializeField]
        private float DyingDelay;

        [SerializeField]
        private float BlinkDelay; // for BlinkTimer

        [SerializeField]
        private float BlinkEffectDuration; // for BlinkEffectLasting

        private List<Sprite> CurrentAnimation;

        private int Index;
        private float Timer;
        private float Delay;

        private float BlinkTimer;
        private float BlinkEffectLasting;

        private delegate void State();
        State CurrentState = () => { };

        private void OnEnable()
        {
            CurrentState = Begin;
        }

        private void OnDisable()
        {
            BlinkRenderer.enabled = false;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void NormalAnimation()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = Delay;

            if (Index < CurrentAnimation.Count - 1)
            {
                Index++;
            }
            else
            {
                Index = 0;
            }

            NormalRenderer.sprite = CurrentAnimation[Index];
        }

        private void BlinkAnimation()
        {
            BlinkEffectLasting -= Time.fixedDeltaTime;
            if (BlinkEffectLasting <= 0)
            {
                BlinkRenderer.enabled = false;
                CurrentState = NormalAnimation;
                return;
            }

            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = Delay;

                if (Index < CurrentAnimation.Count - 1)
                {
                    Index++;
                }
                else
                {
                    Index = 0;
                }

                NormalRenderer.sprite = CurrentAnimation[Index];
                Mask.sprite = CurrentAnimation[Index];
            }

            BlinkTimer -= Time.fixedDeltaTime;

            if (BlinkTimer <= 0)
            {
                BlinkTimer = BlinkDelay;
                BlinkRenderer.enabled = !BlinkRenderer.enabled;
            }
        }

        public void StartBlinking()
        {
            BlinkTimer = BlinkDelay;
            BlinkEffectLasting = BlinkEffectDuration;

            BlinkRenderer.enabled = true;
            Mask.sprite = CurrentAnimation[Index];

            CurrentState = BlinkAnimation;
        }

        public void StopBlinking()
        {
            BlinkTimer = 0f;
            BlinkEffectLasting = 0f;
            BlinkRenderer.enabled = false;
        }

        public void SetFlip(bool flip)
        {
            NormalRenderer.flipX = flip;
            
            if (NormalRenderer.flipX == true)
            {
                Mask.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            if (NormalRenderer.flipX == false)
            {
                Mask.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        public float PlayWalk()
        {
            CurrentAnimation = Walk;
            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            Delay = WalkDelay;
            Timer = Delay;
            Index = 0;

            //CurrentState = PlayAnimation;
            return (CurrentAnimation.Count -1) * Delay;
        }

        public float PlayDying()
        {
            CurrentAnimation = Dying;
            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            Delay = DyingDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayJump()
        {
            CurrentAnimation = Jump;
            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            Delay = 1f;
            Timer = Delay;
            Index = 0;

            //CurrentState = PlayAnimation;
            return (CurrentAnimation.Count - 1) * Delay;
        }


        private void Begin()
        {
            PlayWalk();
            BlinkRenderer.enabled = false;

            CurrentState = NormalAnimation;
        }

        public void Stop()
        {
            CurrentState = () => { };
        }
    }
}
