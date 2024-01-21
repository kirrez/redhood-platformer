using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TentacleAnimator : MonoBehaviour
    {
        //
        [SerializeField]
        private SpriteRenderer NormalRenderer;

        [SerializeField]
        private SpriteRenderer BlinkRenderer;

        [SerializeField]
        private SpriteMask Mask;
        //

        [SerializeField]
        private List<Sprite> EmptyAnimation; // tentacle dead

        [SerializeField]
        private List<Sprite> IdleAnimation; // both

        [SerializeField]
        private List<Sprite> AttackAnimation; // both

        [SerializeField]
        private List<Sprite> RespawnAnimation; // tentacle



        [SerializeField]
        private float IdleDelay = 0.15f;

        [SerializeField]
        private float AttackDelay = 0.075f;

        [SerializeField]
        private float RespawnDelay = 0.15f;

        [SerializeField]
        private float BlinkPeriod; // for BlinkTimer

        [SerializeField]
        private float BlinkEffectDuration; // for BlinkEffectLasting

        private List<Sprite> CurrentAnimation;

        private int Index;
        private float AnimationDelay;
        private float AnimationTimer;

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

        private void Begin()
        {
            PlayIdle();
            BlinkRenderer.enabled = false;

            CurrentState = NormalAnimation;
        }

        private void NormalAnimation()
        {
            AnimationTimer -= Time.fixedDeltaTime;
            if (AnimationTimer > 0) return;

            AnimationTimer = AnimationDelay;

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
                AnimationTimer = AnimationDelay;

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

        public void StopBlinking()
        {
            BlinkTimer = 0f;
            BlinkEffectLasting = 0f;
            BlinkRenderer.enabled = false;
        }

        //private void PlayAnimation()
        //{
        //    AnimationTimer += Time.fixedDeltaTime;

        //    if (AnimationTimer >= AnimationDelay)
        //    {
        //        AnimationTimer -= AnimationDelay;

        //        if (AnimationIndex == CurrentAnimation.Count - 1)
        //        {
        //            AnimationIndex = 0;
        //        }
        //        else if (AnimationIndex < CurrentAnimation.Count - 1)
        //        {
        //            AnimationIndex++;
        //        }

        //        //Renderer.sprite = CurrentAnimation[AnimationIndex];
        //    }
        //}

        public float PlayEmpty()
        {
            CurrentAnimation = EmptyAnimation;
            AnimationDelay = 1f;
            Index = 0;
            AnimationTimer = 0f;

            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            return CurrentAnimation.Count * AnimationDelay;
        }

        public float PlayIdle()
        {
            CurrentAnimation = IdleAnimation;
            AnimationDelay = IdleDelay;
            Index = 0;
            AnimationTimer = 0f;

            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            return (CurrentAnimation.Count - 1) * AnimationDelay;
        }

        public float PlayAttack()
        {
            CurrentAnimation = AttackAnimation;
            AnimationDelay = AttackDelay;
            Index = 0;
            AnimationTimer = 0f;

            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            return (CurrentAnimation.Count - 1) * AnimationDelay;
        }

        public float PlayRespawn()
        {
            CurrentAnimation = RespawnAnimation;
            AnimationDelay = RespawnDelay;
            Index = 0;
            AnimationTimer = 0f;

            NormalRenderer.sprite = CurrentAnimation[0];
            Mask.sprite = CurrentAnimation[0];

            return (CurrentAnimation.Count - 1) * AnimationDelay;
        }
    }
}