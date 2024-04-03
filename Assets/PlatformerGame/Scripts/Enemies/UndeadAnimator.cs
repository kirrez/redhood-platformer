using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class UndeadAnimator : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer Renderer;

        [SerializeField]
        protected Color BlinkMain; // magenta

        [SerializeField]
        protected Color BlinkExtra; // cyan

        protected List<Sprite> CurrentAnimation;

        protected int Index;
        protected float Timer;
        protected float Delay;

        protected float FreezeTimer;
        protected float BlinkTimer;
        protected float BlinkDelay = 0.1f;
        protected float BlinkDuration = 1f;
        protected bool IsBlinkColor;
        protected Color NoColor = new Color(1f, 1f, 1f, 0f);
        protected string TextureName = "_BlendColor";

        protected delegate void State();
        protected State CurrentState = () => { };

        protected void FixedUpdate()
        {
            CurrentState();
        }

        public void StartFreeze(float duration)
        {
            FreezeTimer = duration;
            IsBlinkColor = false;
            Renderer.material.SetColor(TextureName, BlinkMain);

            CurrentState = FreezeAnimation;
        }

        public void SetFlip(bool flip)
        {
            Renderer.flipX = flip;
        }

        public void Begin()
        {
            Renderer.material.SetColor(TextureName, NoColor);
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

            Renderer.sprite = CurrentAnimation[Index];
        }

        protected void FreezeAnimation()
        {
            FreezeTimer -= Time.fixedDeltaTime;

            if (FreezeTimer <= 0)
            {
                //back to normal color
                Renderer.material.SetColor(TextureName, NoColor);

                CurrentState = NormalAnimation;
                return;
            }

            // Blinking effect in final part of Freeze state
            if (FreezeTimer < BlinkDuration)
            {
                BlinkTimer -= Time.fixedDeltaTime;

                if (BlinkTimer <= 0)
                {
                    IsBlinkColor = !IsBlinkColor;
                    if (IsBlinkColor == false)
                    {
                        Renderer.material.SetColor(TextureName, BlinkMain);
                    }
                    if (IsBlinkColor == true)
                    {
                        Renderer.material.SetColor(TextureName, BlinkExtra);
                    }

                    BlinkTimer = BlinkDelay;
                }
            }
        }


    }
}