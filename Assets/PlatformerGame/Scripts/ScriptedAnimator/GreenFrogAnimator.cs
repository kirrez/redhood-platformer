using System;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class GreenFrogAnimator : ScriptedAnimatorBase
    {
        public EGreenFrogAnimations CurrentType { get; private set; }

        public GreenFrogAnimator(SpriteRenderer renderer)
            : base(renderer)
        {

        }

        protected override void LoadSprites()
        {
            AnimationProperties CurrentAsset;
            var resourceManager = CompositionRoot.GetResourceManager();

            var types = Enum.GetValues(typeof(EGreenFrogAnimations));

            foreach (var animationType in types)
            {
                var type = (EGreenFrogAnimations)animationType;
                ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EGreenFrogAnimations>(type);
                CurrentAsset.Animations = asset.Animations;
                CurrentAsset.FramesPerSecond = asset.FramesPerSecond;
                CurrentAsset.Loop = asset.Loop;
                CurrentAsset.Full = asset.Full;

                AnimationDataList.Add(CurrentAsset);

                GameObject.Destroy(asset.gameObject); // destroying created instances
            }
        }

        public float Idle()
        {
            CurrentType = EGreenFrogAnimations.Idle;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float Attack()
        {
            CurrentType = EGreenFrogAnimations.Attack;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpRising()
        {
            CurrentType = EGreenFrogAnimations.JumpRising;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpFalling()
        {
            CurrentType = EGreenFrogAnimations.JumpFalling;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float IdleBlink()
        {
            CurrentType = EGreenFrogAnimations.IdleBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float AttackBlink()
        {
            CurrentType = EGreenFrogAnimations.AttackBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpRisingBlink()
        {
            CurrentType = EGreenFrogAnimations.JumpRisingBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpFallingBlink()
        {
            CurrentType = EGreenFrogAnimations.JumpFallingBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }
    }
}