using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class FadeScreenView : BaseScreenView
    {
        private Image Image;

        private float Timer;
        private float Period;
        private float DelayAfterTime;
        private float DelayBeforeTime;

        private float MaxAlpha;
        private float Alpha;

        private bool IsFadeOut = false;
        private bool IsFadeIn = false;
        private bool IsDelayAfter = false;
        private bool IsDelayBefore = false;

        private Color Transparent = new Color(0f, 0f, 0f, 0f);

        private void Awake()
        {
            Image = GetComponent<Image>();
            //reset basic image
            Image.color = Transparent;
        }

        public void ReleaseAll()
        {
            IsFadeOut = false;
            IsFadeIn = false;
            IsDelayAfter = false;
            IsDelayBefore = false;

            Image.color = Transparent;
        }

        public void FadeIn(Color color, float time)
        {
            //canceling previous Fade-out and Delay, if we have any
            if (IsFadeOut)
            {
                IsFadeOut = false;
                IsDelayAfter = false;
            }

            IsFadeIn = true;

            Timer = time;
            Period = time;

            MaxAlpha = color.a;
            Image.color = color;
        }

        public void FadeOut(Color color, float time)
        {
            //canceling previous Fade-in and Delay, if we have any
            if (IsFadeIn)
            {
                IsFadeIn = false;
                IsDelayAfter = false;
            }

            IsFadeOut = true;
            
            Timer = time;
            Period = time;

            MaxAlpha = color.a;
            Image.color = color;
        }

        public void DelayAfter(float time)
        {
            DelayAfterTime = time;
            IsDelayAfter = true;
        }

        public void DelayBefore(Color color, float time)
        {
            DelayBeforeTime = time;
            IsDelayBefore = true;

            Image.color = color;
        }

        private void UpdateDelayBefore()
        {
            if (!IsDelayBefore) return;

            DelayBeforeTime -= Time.deltaTime;

            if (DelayBeforeTime <= 0)
            {
                IsDelayBefore = false;
            }
            //Fade-in or Fade-out won't start until DelayBefore' period ends
            return;
        }

        private void UpdateFadeIn()
        {
            if (!IsFadeIn || IsDelayBefore) return;

            if (Timer <= 0)
            {
                if (IsDelayAfter)
                {
                    DelayAfterTime -= Time.deltaTime;
                    if (DelayAfterTime <= 0)
                    {
                        IsDelayAfter = false;
                    }
                }

                if (!IsDelayAfter)
                {
                    IsFadeIn = false;
                    Image.color = Transparent;
                    return;
                }
            }

            Timer -= Time.deltaTime;
            Alpha = MaxAlpha * (1f - Timer / Period);

            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Alpha);
        }

        private void UpdateFadeOut()
        {
            if (!IsFadeOut || IsDelayBefore) return;

            if (Timer <= 0)
            {
                if (IsDelayAfter)
                {
                    DelayAfterTime -= Time.deltaTime;
                    if (DelayAfterTime <= 0)
                    {
                        IsDelayAfter = false;
                    }
                }

                if (!IsDelayAfter)
                {
                    IsFadeOut = false;
                    Image.color = Transparent;
                    return;
                }
            }

            Timer -= Time.deltaTime;
            Alpha = MaxAlpha * Timer / Period;

            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Alpha);
        }

        private void Update()
        {
            UpdateDelayBefore();
            UpdateFadeIn();
            UpdateFadeOut();
        }
    }
}