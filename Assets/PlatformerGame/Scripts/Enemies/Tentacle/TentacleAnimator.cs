using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TentacleAnimator : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> EmptySprite; // for dead tentacle

        [SerializeField]
        private List<Sprite> IdleAnimation; // both

        [SerializeField]
        private List<Sprite> AttackAnimation; // both

        [SerializeField]
        private List<Sprite> DyingAnimation; // for dying receptacle

        [SerializeField]
        private float IdleDelay = 0.15f;

        [SerializeField]
        private float AttackDelay = 0.075f;

        [SerializeField]
        private float DyingDelay = 0.15f;

        private List<Sprite> CurrentAnimation;

        private int AnimationIndex;
        private float AnimationDelay = 0.075f;
        private float AnimationTimer;

        private SpriteRenderer Renderer;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            CurrentAnimation = IdleAnimation;
            AnimationDelay = IdleDelay;
        }

        private void FixedUpdate()
        {
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            AnimationTimer += Time.fixedDeltaTime;

            if (AnimationTimer >= AnimationDelay)
            {
                AnimationTimer -= AnimationDelay;

                if (AnimationIndex == CurrentAnimation.Count - 1)
                {
                    AnimationIndex = 0;
                }
                else if (AnimationIndex < CurrentAnimation.Count - 1)
                {
                    AnimationIndex++;
                }

                Renderer.sprite = CurrentAnimation[AnimationIndex];
            }
        }

        public void PlayIdle()
        {
            CurrentAnimation = IdleAnimation;
            AnimationDelay = IdleDelay;
            AnimationIndex = 0;
            AnimationTimer = 0f;
        }

        public void PlayAttack()
        {
            CurrentAnimation = AttackAnimation;
            AnimationDelay = AttackDelay;
            AnimationIndex = 0;
            AnimationTimer = 0f;
        }

        public void PlayEmpty()
        {
            CurrentAnimation = EmptySprite;
            AnimationDelay = 0f;
            AnimationIndex = 0;
            AnimationTimer = 0f;
        }
    }
}