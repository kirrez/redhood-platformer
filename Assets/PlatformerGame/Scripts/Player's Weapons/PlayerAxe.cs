using System.Collections.Generic;
using Platformer.ScriptedAnimator;
using UnityEngine;

namespace Platformer
{
    public class PlayerAxe : MonoBehaviour
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

        public void Initiate(Vector2 newPosition, float direction)
        {
            transform.position = newPosition;

            if (direction == 1f)
            {
                Renderer.flipX = false;
            }
            if (direction == -1f)
            {
                Renderer.flipX = true;
            }
        }

        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();

            ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.PlayerAxe);
            CurrentAnimations = asset.Animations;
            AnimationDelay = 1 / asset.FramesPerSecond;

            GameObject.Destroy(asset.gameObject);
        }
    }
}