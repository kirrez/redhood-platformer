using Platformer.ScriptedAnimator;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DoorAnimator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer Renderer;

        private AnimationProperties Open;
        private AnimationProperties Close;

        private List<Sprite> CurrentAnimations;
        private EGFXAnimations Type;
        private float AnimationDelay = 0f;
        private int SpriteIndex;
        private float Timer;

        private void Awake()
        {
            LoadSprites();
        }

        private void FixedUpdate()
        {
            if (Type == EGFXAnimations.DoorRedOpen || Type == EGFXAnimations.DoorRedClose)
            {
                Renderer.sprite = CurrentAnimations[SpriteIndex];

                Timer += Time.deltaTime;

                if (Timer >= AnimationDelay)
                {
                    Timer -= AnimationDelay;

                    if (SpriteIndex < CurrentAnimations.Count - 1)
                    {
                        SpriteIndex++;
                    }
                }
            }
        }

        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();

            ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.DoorRedOpen);
            Open.Animations = asset.Animations;
            Open.FramesPerSecond = asset.FramesPerSecond;
            GameObject.Destroy(asset.gameObject);

            asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.DoorRedClose);
            Close.Animations = asset.Animations;
            Close.FramesPerSecond = asset.FramesPerSecond;
            GameObject.Destroy(asset.gameObject);
        }

        public void AnimateOpen()
        {
            if (Type == EGFXAnimations.DoorRedOpen) return;

            CurrentAnimations = Open.Animations;
            AnimationDelay = 1 / Open.FramesPerSecond;
            Timer = 0f;
            SpriteIndex = 0;
            Type = EGFXAnimations.DoorRedOpen;
        }

        public void AnimateClose()
        {
            if (Type == EGFXAnimations.DoorRedClose) return;

            CurrentAnimations = Close.Animations;
            AnimationDelay = 1 / Close.FramesPerSecond;
            Timer = 0f;
            SpriteIndex = 0;
            Type = EGFXAnimations.DoorRedClose;
        }
    }
}