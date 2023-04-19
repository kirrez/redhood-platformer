using System;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class EvilBearAnimator : ScriptedAnimatorBase
    {
        public EEvilBearAnimations CurrentType { get; private set; }

        public EvilBearAnimator(SpriteRenderer renderer)
            : base(renderer)
        {

        }

        protected override void LoadSprites()
        {
            AnimationProperties CurrentAsset;
            var resourceManager = CompositionRoot.GetResourceManager();

            var types = Enum.GetValues(typeof(EEvilBearAnimations));

            foreach (var animationType in types)
            {
                var type = (EEvilBearAnimations)animationType;
                ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EEvilBearAnimations>(type);
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
            CurrentType = EEvilBearAnimations.Idle;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float IdleBlink()
        {
            CurrentType = EEvilBearAnimations.IdleBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float Walk()
        {
            CurrentType = EEvilBearAnimations.Walk;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float WalkBlink()
        {
            CurrentType = EEvilBearAnimations.WalkBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Attack()
        {
            CurrentType = EEvilBearAnimations.Attack;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float AttackBlink()
        {
            CurrentType = EEvilBearAnimations.AttackBlink;
            var index = (int)CurrentType;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }
    }
}