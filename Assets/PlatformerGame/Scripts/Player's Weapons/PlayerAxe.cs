using System.Collections.Generic;
using Platformer.ScriptedAnimator;
using UnityEngine;

namespace Platformer
{
    public class PlayerAxe : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 3)]
        private int WeaponLevel = 1;

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

        public void Initiate(Vector2 newPosition, EFacing direction)
        {
            transform.position = newPosition;

            if (direction == EFacing.Right)
            {
                Renderer.flipX = false;
            }
            if (direction == EFacing.Left)
            {
                Renderer.flipX = true;
            }
        }

        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            ScriptedAnimation asset = null;

            switch (WeaponLevel)
            {
                case 1:
                    asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.CrippledAxe);
                    break;
                case 2:
                    asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.SharpenedAxe);
                    break;
                case 3:
                    asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.SturdyAxe);
                    break;
            }

            //asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.PlayerAxe);
            CurrentAnimations = asset.Animations;
            AnimationDelay = 1 / asset.FramesPerSecond;

            GameObject.Destroy(asset.gameObject);
        }
    }
}