using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class PlayerAnimator : ScriptedAnimatorBase
    {
        public PlayerAnimator(SpriteRenderer renderer)
            : base(renderer)
        {
            
        }

        protected override void LoadSprites()
        {
            // animations for Player in folder "EScriptedAnimations"
            AnimationProperties CurrentAsset;
            var resourceManager = CompositionRoot.GetResourceManager();

            var types = Enum.GetValues(typeof(EPlayerAnimations));

            foreach (var animationType in types)
            {
                var type = (EPlayerAnimations)animationType;
                ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EPlayerAnimations>(type);
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
            var index = (int)EPlayerAnimations.Idle;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float Walk()
        {
            var index = (int)EPlayerAnimations.Walk;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float JumpRising()
        {
            var index = (int)EPlayerAnimations.JumpRising;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float JumpFalling()
        {
            var index = (int)EPlayerAnimations.JumpFalling;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Dying()
        {
            var index = (int)EPlayerAnimations.Dying;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Attack()
        {
            var index = (int)EPlayerAnimations.Attack;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float AirAttack()
        {
            var index = (int)EPlayerAnimations.AirAttack;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Sit()
        {
            var index = (int)EPlayerAnimations.Sit;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float SitAttack()
        {
            var index = (int)EPlayerAnimations.SitAttack;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float RollDown()
        {
            var index = (int)EPlayerAnimations.RollDown;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Crouch()
        {
            var index = (int)EPlayerAnimations.Crouch;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float DamageTaken()
        {
            var index = (int)EPlayerAnimations.DamageTaken;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float SitDamageTaken()
        {
            var index = (int)EPlayerAnimations.SitDamageTaken;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Stunned()
        {
            var index = (int)EPlayerAnimations.Stunned;
            LoadNextAnimation(index);
            return NextAnimationDelay * NextAnimations.Count;
        }
    }
}

