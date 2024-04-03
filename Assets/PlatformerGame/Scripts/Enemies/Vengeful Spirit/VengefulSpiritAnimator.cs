using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class VengefulSpiritAnimator : UndeadAnimator
    {
        [SerializeField]
        private List<Sprite> Floating;
        
        [SerializeField]
        private List<Sprite> Cautious;
        
        [SerializeField]
        private List<Sprite> Pursuing;

        [SerializeField]
        private float AnimationDelay;

        public float PlayFloating()
        {
            CurrentAnimation = Floating;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayCautious()
        {
            CurrentAnimation = Cautious;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }

        public float PlayPursuing()
        {
            CurrentAnimation = Pursuing;
            Renderer.sprite = CurrentAnimation[0];

            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;

            return (CurrentAnimation.Count - 1) * Delay;
        }
    }
}