using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public abstract class ScriptedAnimatorBase
    {
        protected SpriteRenderer Renderer;
        protected List<Sprite> CurrentAnimations;
        protected float AnimationDelay = 0.1f;
        protected float Timer;
        protected bool Loop = false;
        protected bool Full = false;
        protected int SpriteIndex;

        protected List<Sprite> NextAnimations;
        protected float NextAnimationDelay;
        protected bool NextLoop;
        protected bool NextFull;

        protected bool NextAnimationSwitched = false;

        protected List<AnimationProperties> AnimationDataList = new List<AnimationProperties>();

        public ScriptedAnimatorBase(SpriteRenderer renderer)
        {
            Renderer = renderer;
            LoadSprites();
        }

        protected abstract void LoadSprites();

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
            Timer += Time.deltaTime;

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
                    SpriteIndex++;
                }
            }
        }

        protected void SetupAnimation()
        {
            Timer = 0f;
            SpriteIndex = 0;
            CurrentAnimations = NextAnimations;
            AnimationDelay = NextAnimationDelay;
            Loop = NextLoop;
            Full = NextFull;
            NextAnimationSwitched = false;
        }

        protected void LoadNextAnimation(int index)
        {
            NextAnimations = AnimationDataList[index].Animations;
            NextAnimationDelay = 1 / AnimationDataList[index].FramesPerSecond;
            NextLoop = AnimationDataList[index].Loop;
            NextFull = AnimationDataList[index].Full;
            NextAnimationSwitched = true;
        }

    }
}