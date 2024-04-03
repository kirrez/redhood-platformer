using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TentacleAnimator : BlinkAnimator
    {
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

        public float PlayEmpty()
        {
            CurrentAnimation = EmptyAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = 1f;
            Timer = Delay;
            Index = 0;

            return 1f;
        }

        public float PlayIdle()
        {
            CurrentAnimation = IdleAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = IdleDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayAttack()
        {
            CurrentAnimation = AttackAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AttackDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayRespawn()
        {
            CurrentAnimation = RespawnAnimation;
            Renderer.sprite = CurrentAnimation[0];

            Delay = RespawnDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }
    }
}