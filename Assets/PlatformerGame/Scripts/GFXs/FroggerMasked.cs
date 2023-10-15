using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FroggerMasked : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private SpriteRenderer BlinkRenderer;

        [SerializeField]
        private SpriteMask Mask;

        private int Index;

        private float Timer;
        private float Delay = 0.25f;

        private float BlinkTimer;
        private float BlinkDelay = 0.1f;

        private delegate void State();
        State CurrentState = () => { };

        private void OnEnable()
        {
            CurrentState = Begin;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void Begin()
        {
            Timer = Delay;
            Index = 0;
            BlinkTimer = BlinkDelay;

            BlinkRenderer.enabled = false;

            CurrentState = NormalAnimation;
        }

        private void NormalAnimation()
        {
            BlinkTimer -= Time.fixedDeltaTime;
            if (BlinkTimer <= 0)
            {
                BlinkTimer = BlinkDelay;
                BlinkRenderer.enabled = true;
                Mask.sprite = Sprites[Index];
                CurrentState = BlinkAnimation;
                return;
            }

            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = Delay;

            if (Index < Sprites.Count - 1)
            {
                Index++;
            }
            else
            {
                Index = 0;
            }

            Renderer.sprite = Sprites[Index];
        }

        private void BlinkAnimation()
        {
            BlinkTimer -= Time.fixedDeltaTime;
            if (BlinkTimer <= 0)
            {
                BlinkTimer = BlinkDelay;
                BlinkRenderer.enabled = false;
                CurrentState = NormalAnimation;
                return;
            }

            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = Delay;

            if (Index < Sprites.Count - 1)
            {
                Index++;
            }
            else
            {
                Index = 0;
            }

            Renderer.sprite = Sprites[Index];
            Mask.sprite = Sprites[Index];
        }
    }
}