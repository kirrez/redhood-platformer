using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class UndeadAnimator : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer NormalRenderer;

        [SerializeField]
        protected SpriteRenderer FreezeRenderer;

        [SerializeField]
        protected SpriteMask Mask;

        [SerializeField]
        protected List<Sprite> FreezeColors;

        protected List<Sprite> CurrentAnimation;

        protected int Index;
        protected float Timer;
        protected float Delay;

        protected float FreezeTimer;
        protected float BlinkTimer;
        protected float BlinkDelay = 0.1f;
        protected float BlinkDuration = 1f;
        protected int ColorIndex;

        protected delegate void State();
        protected State CurrentState = () => { };

        protected void OnDisable()
        {
            FreezeRenderer.enabled = false;
        }

        protected void FixedUpdate()
        {
            CurrentState();
        }

        public void StartFreeze(float duration)
        {
            FreezeTimer = duration;
            ColorIndex = 0;

            FreezeRenderer.enabled = true;
            Mask.sprite = CurrentAnimation[Index];
            FreezeRenderer.sprite = FreezeColors[ColorIndex];

            CurrentState = FreezeAnimation;
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

        public void Begin()
        {
            //need to load some sprites first
            FreezeRenderer.enabled = false;
            CurrentState = NormalAnimation;
        }

        public void Stop()
        {
            CurrentState = () => { };
        }

        protected void NormalAnimation()
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

        protected void FreezeAnimation()
        {
            FreezeTimer -= Time.fixedDeltaTime;

            if (FreezeTimer <= 0)
            {
                FreezeRenderer.enabled = false;
                CurrentState = NormalAnimation;
            }

            // Blinking effect in final part of Freeze state
            if (FreezeTimer < BlinkDuration)
            {
                BlinkTimer -= Time.fixedDeltaTime;

                if (BlinkTimer <= 0)
                {
                    if (ColorIndex < FreezeColors.Count - 1)
                    {
                        ColorIndex++;
                    }
                    else
                    {
                        ColorIndex = 0;
                    }

                    FreezeRenderer.sprite = FreezeColors[ColorIndex];
                    BlinkTimer = BlinkDelay;
                }
            }
        }


    }
}