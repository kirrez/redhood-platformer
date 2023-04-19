using UnityEngine;
using Platformer.ScriptedAnimator;
using System.Collections.Generic;

namespace Platformer
{
    public class Candle : MonoBehaviour
    {
        private SpriteRenderer Renderer;
        private List<Sprite> CurrentAnimations;
        private float AnimationDelay = 0.1f;
        private int SpriteIndex;
        private float Timer;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            LoadSprites();
        }

        private void OnEnable()
        {
            Timer = 0f;
            SpriteIndex = 0;
        }

        private void FixedUpdate()
        {
            Renderer.sprite = CurrentAnimations[SpriteIndex];
            Timer += Time.deltaTime;

            if (Timer >= AnimationDelay)
            {
                Timer -= AnimationDelay;

                if (SpriteIndex == CurrentAnimations.Count - 1)
                {
                    SpriteIndex = 0;
                    return;
                }

                if (SpriteIndex < CurrentAnimations.Count - 1)
                {
                    SpriteIndex++;
                }
            }
        }

        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();

            ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.Candle);
            CurrentAnimations = asset.Animations;
            AnimationDelay = 1 / asset.FramesPerSecond;

            GameObject.Destroy(asset.gameObject);
        }
    }
}