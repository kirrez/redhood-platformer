using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BlinkAnimator : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer Renderer;

        [SerializeField]
        protected Color BlinkMain; // white

        [SerializeField]
        protected float EffectDuration;

        [SerializeField]
        protected float BlinkDelay;

        protected List<Sprite> CurrentAnimation;

        protected int Index;
        protected float Timer;
        protected float Delay;

        protected float BlinkTimer;
        protected float EffectTimer;
        protected bool IsBlinkColor;

        protected Color NoColor = new Color(1f, 1f, 1f, 0f);
        protected string TextureName = "_BlendColor";

        protected delegate void State();
        protected State CurrentState = () => { };

        public void Begin()
        {
            Renderer.material.SetColor(TextureName, NoColor);
            CurrentState = NormalAnimation;
        }

        public void Stop()
        {
            CurrentState = () => { };
        }

        public void SetFlip(bool flip)
        {
            Renderer.flipX = flip;
        }

        public void StartBlinking()
        {
            BlinkTimer = BlinkDelay;
            EffectTimer = EffectDuration;
            Renderer.material.SetColor(TextureName, BlinkMain);

            CurrentState = BlinkAnimation;
        }

        public void StopBlinking()
        {
            BlinkTimer = 0f;
            EffectTimer = 0f;
            Renderer.material.SetColor(TextureName, NoColor);
        }

        protected void FixedUpdate()
        {
            CurrentState();
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

        private void BlinkAnimation()
        {
            EffectTimer -= Time.fixedDeltaTime;

            if (EffectTimer <= 0)
            {
                Renderer.material.SetColor(TextureName, NoColor);
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

                Renderer.sprite = CurrentAnimation[Index];
            }

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
                    Renderer.material.SetColor(TextureName, NoColor);
                }

                BlinkTimer = BlinkDelay;
            }
        }

    }
}