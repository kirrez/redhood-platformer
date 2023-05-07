using System.Collections;
using System.Collections.Generic;
using Platformer.ScriptedAnimator;
using UnityEngine;

namespace Platformer
{
    public class BearSlash : MonoBehaviour
    {
        private SpriteRenderer Renderer;
        private Collider2D Collider;
        private List<Sprite> CurrentAnimations;
        private float AnimationDelay = 0.1f;
        private float Timer;
        private int SpriteIndex;
        private float MaxLifeTime;
        private float LifeTime;
        private float CollisionTime;

        public void SetHitDirection(float direction)
        {
            GetComponent<DamageDealer>().SetHitDirection(direction);
            if (direction == 1)
            {
                Renderer.flipX = false;
            }

            if (direction == -1)
            {
                Renderer.flipX = true;
            }
        }

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<Collider2D>();

            LoadSprites();
        }

        private void OnEnable()
        {
            Collider.enabled = true;
            Timer = 0f;
            SpriteIndex = 0;
            LifeTime = 0f;
            CollisionTime = 0.5f; // magic number
        }

        private void Update()
        {
            Renderer.sprite = CurrentAnimations[SpriteIndex];

            Timer += Time.deltaTime;
            LifeTime += Time.deltaTime;

            if (Timer >= AnimationDelay)
            {
                Timer -= AnimationDelay;

                if (SpriteIndex < CurrentAnimations.Count - 1)
                {
                    SpriteIndex++;
                }
            }

            if (LifeTime >= CollisionTime)
            {
                Collider.enabled = false;
            }

            if (LifeTime >= MaxLifeTime)
            {
                gameObject.SetActive(false);
            }
        }


        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();

            ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.BearSlash);
            CurrentAnimations = asset.Animations;
            AnimationDelay = 1 / asset.FramesPerSecond;
            MaxLifeTime = AnimationDelay * asset.FramesPerSecond;

            GameObject.Destroy(asset.gameObject);
        }
    }
}