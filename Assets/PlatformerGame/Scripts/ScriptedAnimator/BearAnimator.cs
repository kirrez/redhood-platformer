using System;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class BearAnimator : ScriptedAnimatorBase
    {
        public EBearAnimations CurrentType { get; private set; }

        public BearAnimator(SpriteRenderer renderer)
            : base(renderer)
        {

        }

        protected override void LoadSprites()
        {
            AnimationProperties CurrentAsset;
            var resourceManager = CompositionRoot.GetResourceManager();

            var types = Enum.GetValues(typeof(EBearAnimations));

            foreach (var animationType in types)
            {
                var type = (EBearAnimations)animationType;
                ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EBearAnimations>(type);
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
            CurrentType = EBearAnimations.Idle;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float IdleBlink()
        {
            CurrentType = EBearAnimations.IdleBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float Walk()
        {
            CurrentType = EBearAnimations.Walk;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float WalkBlink()
        {
            CurrentType = EBearAnimations.WalkBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Attack()
        {
            CurrentType = EBearAnimations.Attack;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float AttackBlink()
        {
            CurrentType = EBearAnimations.AttackBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }
    }
}