using System;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class FrogAnimator : ScriptedAnimatorBase
    {
        public EFrogAnimations CurrentType { get; private set; }

        public FrogAnimator(SpriteRenderer renderer)
            : base(renderer)
        {

        }

        protected override void LoadSprites()
        {
            AnimationProperties CurrentAsset;
            var resourceManager = CompositionRoot.GetResourceManager();

            var types = Enum.GetValues(typeof(EFrogAnimations));

            foreach (var animationType in types)
            {
                var type = (EFrogAnimations)animationType;
                ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EFrogAnimations>(type);
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
            CurrentType = EFrogAnimations.Idle;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float Attack()
        {
            CurrentType = EFrogAnimations.Attack;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpRising()
        {
            CurrentType = EFrogAnimations.JumpRising;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpFalling()
        {
            CurrentType = EFrogAnimations.JumpFalling;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float IdleBlink()
        {
            CurrentType = EFrogAnimations.IdleBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float AttackBlink()
        {
            CurrentType = EFrogAnimations.AttackBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpRisingBlink()
        {
            CurrentType = EFrogAnimations.JumpRisingBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float JumpFallingBlink()
        {
            CurrentType = EFrogAnimations.JumpFallingBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }
    }
}