using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer
{
    public class CapAnimator : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> IdleAnimation;

        [SerializeField]
        private List<Sprite> ClosingAnimation; // after tentacle's destruction

        [SerializeField]
        private List<Sprite> OpeningAnimation; // before tentacle respawn

        [SerializeField]
        private List<Sprite> HybernatingAnimation; // while tentacle dead



        [SerializeField]
        private float IdleDelay = 0.15f;

        [SerializeField]
        private float ClosingDelay = 0.15f;

        [SerializeField]
        private float OpeningDelay = 0.15f;

        [SerializeField]
        private float HybernatingDelay = 0.15f;

        private List<Sprite> CurrentAnimation;

        private int Index;
        private float AnimationDelay;
        private float AnimationTimer;

        private SpriteRenderer Renderer;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            PlayIdle();
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

                if (Index == CurrentAnimation.Count - 1)
                {
                    Index = 0;
                }

                else if (Index < CurrentAnimation.Count - 1)
                {
                    Index++;
                }

                Renderer.sprite = CurrentAnimation[Index];
            }
        }

        public float PlayIdle()
        {
            CurrentAnimation = IdleAnimation;
            AnimationDelay = IdleDelay;
            Index = 0;
            AnimationTimer = 0f;

            return (CurrentAnimation.Count - 1) * AnimationDelay;
        }

        public float PlayClosing()
        {
            CurrentAnimation = ClosingAnimation;
            AnimationDelay = ClosingDelay;
            Index = 0;
            AnimationTimer = 0f;

            return (CurrentAnimation.Count - 1) * AnimationDelay;
        }

        public float PlayOpening()
        {
            CurrentAnimation = OpeningAnimation;
            AnimationDelay = OpeningDelay;
            Index = 0;
            AnimationTimer = 0f;

            return (CurrentAnimation.Count - 1) * AnimationDelay;
        }

        public float PlayHybernating()
        {
            CurrentAnimation = HybernatingAnimation;
            AnimationDelay = HybernatingDelay;
            Index = 0;
            AnimationTimer = 0f;

            return (CurrentAnimation.Count - 1) * AnimationDelay;
        }
    }
}