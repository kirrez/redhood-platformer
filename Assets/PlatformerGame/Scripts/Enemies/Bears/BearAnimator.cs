using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public enum BearAnimations
    {
        Idle,
        Walk,
        Attack
    }
    public class BearAnimator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer NormalRenderer;

        [SerializeField]
        private SpriteRenderer BlinkRenderer;

        [SerializeField]
        private SpriteMask Mask;

        [SerializeField]
        private List<Sprite> Idle;

        [SerializeField]
        private List<Sprite> Walk;

        [SerializeField]
        private List<Sprite> Attack;

        [SerializeField]
        private float AnimationPeriod;

        [SerializeField]
        private float BlinkPeriod; // for BlinkTimer

        [SerializeField]
        private float BlinkEffectDuration; // for BlinkEffectLasting

        private float AnimationTimer;

        private float BlinkTimer;
        private float BlinkEffectLasting;

        private int Index;

        private List<Sprite> CurrentAnimation;

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

        private void Begin()
        {
            Index = 0;
            AnimationTimer = AnimationPeriod;
            SetAnimation(BearAnimations.Walk);
            Mask.sprite = CurrentAnimation[0];
            BlinkRenderer.enabled = false;

            CurrentState = NormalAnimation;
        }

        private void NormalAnimation()
        {
            AnimationTimer -= Time.fixedDeltaTime;
            if (AnimationTimer > 0) return;

            AnimationTimer = AnimationPeriod;

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

            AnimationTimer -= Time.fixedDeltaTime;

            if (AnimationTimer <= 0)
            {
                AnimationTimer = AnimationPeriod;

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
                BlinkTimer = BlinkPeriod;
                BlinkRenderer.enabled = !BlinkRenderer.enabled;
            }
        }


        public void StartBlinking()
        {
            BlinkTimer = BlinkPeriod;
            BlinkEffectLasting = BlinkEffectDuration;

            BlinkRenderer.enabled = true;
            Mask.sprite = CurrentAnimation[Index];

            CurrentState = BlinkAnimation;
        }

        public void SetAnimation(BearAnimations animation)
        {
            switch (animation)
            {
                case BearAnimations.Idle:
                    CurrentAnimation = Idle;
                    break;

                case BearAnimations.Walk:
                    CurrentAnimation = Walk;
                    break;

                case BearAnimations.Attack:
                    CurrentAnimation = Attack;
                    break;
            }

            Index = 0;
            NormalRenderer.sprite = CurrentAnimation[Index];
            Mask.sprite = CurrentAnimation[Index];
        }

        public void SetFlip(bool flip)
        {
            NormalRenderer.flipX = flip;
            AlignFlips();
        }

        private void AlignFlips()
        {
            if (NormalRenderer.flipX)
            {
                Mask.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            if (!NormalRenderer.flipX)
            {
                Mask.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}