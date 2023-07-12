using Platformer.ScriptedAnimator;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class WeaponDisappear : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 3)]
        private int WeaponType = 1;

        private SpriteRenderer Renderer;
        private List<Sprite> CurrentAnimations;
        private float AnimationDelay = 0.1f;
        private int SpriteIndex;
        private float MaxLifeTime;
        private float LifeTime;
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
            LifeTime = 0f;
        }

        private void FixedUpdate()
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

            if (LifeTime >= MaxLifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        public void Initiate(Vector2 newPosition)
        {
            transform.position = newPosition;
        }

        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            ScriptedAnimation asset = null;

            switch (WeaponType)
            {
                case 1:
                    asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.HolyWaterDisappear);
                    break;
                case 2:
                    asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.HolyWaterDisappear);
                    break;
                case 3:
                    asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.HolyWaterDisappear);
                    break;
            }

            CurrentAnimations = asset.Animations;
            AnimationDelay = 1 / asset.FramesPerSecond;
            MaxLifeTime = AnimationDelay * asset.FramesPerSecond;

            GameObject.Destroy(asset.gameObject);
        }
    }
}