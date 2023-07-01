using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class PlayerScriptedAnimations
    {
        private SpriteRenderer Renderer;
        private List<Sprite> CurrentAnimations;
        private float AnimationDelay = 0.1f;
        private float Timer;
        private bool Loop = false;
        private bool Full = false;
        private int SpriteIndex;

        //Next animation properties (for "Full" option)
        private List<Sprite> NextAnimations;
        private float NextAnimationDelay;
        private bool NextLoop;
        private bool NextFull;

        private bool NextAnimationSwitched = false;

        private List<AnimationProperties> AnimationDataList = new List<AnimationProperties>(); 

        public PlayerScriptedAnimations(SpriteRenderer renderer)
        {
            Renderer = renderer;

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

        private void SetupAnimation()
        {
            Timer = 0f;
            SpriteIndex = 0;
            CurrentAnimations = NextAnimations;
            AnimationDelay = NextAnimationDelay;
            Loop = NextLoop;
            Full = NextFull;
            NextAnimationSwitched = false;
        }

        public void Update()
        {
            // first run
            if (CurrentAnimations == null)
            {
                SetupAnimation();
            }
            //
            if (NextAnimationSwitched && !Full)
            {
                SetupAnimation();
            }


            Renderer.sprite = CurrentAnimations[SpriteIndex];
            Timer += Time.fixedDeltaTime;

            if (Timer >= AnimationDelay)
            {
                Timer -= AnimationDelay;

                if (SpriteIndex == CurrentAnimations.Count - 1)
                {
                    // if not Full and not Loop, then animation just stops
                    if (NextAnimationSwitched && Full)
                    {
                        SetupAnimation();
                    }
                    if (Loop)
                    {
                        SpriteIndex = 0;
                    }
                }

                if (SpriteIndex < CurrentAnimations.Count - 1)
                {
                    //if (!NextAnimationSwitched)
                    //{
                    //    SpriteIndex++;
                    //}

                    //if (NextAnimationSwitched && Full)
                    //{
                    //    SpriteIndex++;
                    //}
                    SpriteIndex++;
                }
                
                
            }

            
        }

        public float Idle()
        {
            var index = (int)EPlayerAnimations.Idle;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count; // animation time
        }

        public float Walk()
        {
            //Timer = 0f;
            //SpriteIndex = 0;
            //var index = (int)EPlayerAnimations.Walk;

            //CurrentAnimations = AnimationDataList[index].Animations;
            //AnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            //Loop = AnimationDataList[index].Loop;
            //return AnimationDelay * CurrentAnimations.Count;

            var index = (int)EPlayerAnimations.Walk;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float JumpRising()
        {
            var index = (int)EPlayerAnimations.JumpRising;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float JumpFalling()
        {
            var index = (int)EPlayerAnimations.JumpFalling;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Dying()
        {
            var index = (int)EPlayerAnimations.Dying;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Attack()
        {
            var index = (int)EPlayerAnimations.Attack;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float AirAttack()
        {
            var index = (int)EPlayerAnimations.AirAttack;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Sit()
        {
            var index = (int)EPlayerAnimations.Sit;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float SitAttack()
        {
            var index = (int)EPlayerAnimations.SitAttack;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float RollDown()
        {
            var index = (int)EPlayerAnimations.RollDown;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Crouch()
        {
            var index = (int)EPlayerAnimations.Crouch;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float DamageTaken()
        {
            var index = (int)EPlayerAnimations.DamageTaken;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float SitDamageTaken()
        {
            var index = (int)EPlayerAnimations.SitDamageTaken;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }

        public float Stunned()
        {
            var index = (int)EPlayerAnimations.Stunned;
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
            return NextAnimationDelay * NextAnimations.Count;
        }
    }
}