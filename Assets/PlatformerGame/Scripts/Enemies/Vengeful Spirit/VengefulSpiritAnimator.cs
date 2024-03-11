using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class VengefulSpiritAnimator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer NormalRenderer;

        [SerializeField]
        private SpriteRenderer FreezeRenderer;

        [SerializeField]
        private SpriteMask Mask;

        [SerializeField]
        private List<Sprite> Floating;
        
        [SerializeField]
        private List<Sprite> Cautious;
        
        [SerializeField]
        private List<Sprite> Pursuing;

        [SerializeField]
        private float AnimationDelay;

        private List<Sprite> CurrentAnimation;

        private int Index;
        private float Timer;
        private float Delay;

        private float FreezeTimer;

        private delegate void State();
        State CurrentState = () => { };

        private void OnEnable()
        {
            CurrentState = Begin;
        }

        private void OnDisable()
        {
            FreezeRenderer.enabled = false;
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

        private void FreezeAnimation()
        {
            FreezeTimer -= Time.fixedDeltaTime;
            
            if (FreezeTimer <= 0)
            {
                FreezeRenderer.enabled = false;
                CurrentState = NormalAnimation;
            }
        }

        public void StartFreeze(float duration)
        {
            FreezeTimer = duration;

            FreezeRenderer.enabled = true;
            Mask.sprite = CurrentAnimation[Index];

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

        public float PlayFloating()
        {
            CurrentAnimation = Floating;
            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayCautious()
        {
            CurrentAnimation = Cautious;
            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayPursuing()
        {
            CurrentAnimation = Pursuing;
            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        private void Begin()
        {
            PlayFloating();
            FreezeRenderer.enabled = false;

            CurrentState = NormalAnimation;
        }

        public void Stop()
        {
            CurrentState = () => { };
        }
    }
}