using System.Collections;
using System.Collections.Generic;
using Platformer.ScriptedAnimator;
using UnityEngine;

namespace Platformer
{
    public class BreakableWood : MonoBehaviour
    {
        private SpriteRenderer Renderer;
        private List<Sprite> CurrentAnimations;
        private float AnimationDelay = 0.1f;
        private int SpriteIndex;
        private float Timer;
        private float AnimationTimer;

        private IResourceManager ResourceManager;

        private bool IsBeignHit;

        [SerializeField]
        private Collider2D SolidCollider;

        [SerializeField]
        private Sprite UnbrokenSprite;

        [SerializeField]
        private Sprite BrokenSprite;

        delegate void StateMethod();
        StateMethod CurrentState;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            ResourceManager = CompositionRoot.GetResourceManager();
            LoadSprites();
        }

        private void OnEnable()
        {
            Timer = 0f;
            SpriteIndex = 0;
            Renderer.sprite = UnbrokenSprite;
            IsBeignHit = false;
            SolidCollider.enabled = true;
            CurrentState = UnbrokenState;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void UnbrokenState()
        {
            if (IsBeignHit)
            {
                Timer = 3f;
                Renderer.sprite = BrokenSprite;
                SolidCollider.enabled = false;
                // shatters
                for (int i = 0; i < 3; i++)
                {
                    var shatter = ResourceManager.GetFromPool(GFXs.WoodShatter);
                    shatter.GetComponent<WoodShatter>().Initiate(this.transform.position);
                }

                CurrentState = BrokenState;
            }
        }

        private void BrokenState()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = 3f;
                CurrentState = AssemblingState;
            }
        }

        private void AssemblingState()
        {
            Timer -= Time.fixedDeltaTime;

            AnimationTimer += Time.fixedDeltaTime;

            if (AnimationTimer >= AnimationDelay)
            {
                AnimationTimer -= AnimationDelay;
                if (SpriteIndex < CurrentAnimations.Count - 1)
                {
                    SpriteIndex++;
                    Renderer.sprite = CurrentAnimations[SpriteIndex];
                }
            }

            if (Timer <= 0)
            {
                // temporary
                Renderer.sprite = UnbrokenSprite;
                SolidCollider.enabled = true;
                IsBeignHit = false;
                SpriteIndex = 0;
                CurrentState = UnbrokenState;
            }
        }

        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();
            ScriptedAnimation asset = null;
            asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.BreakableWoodAssembling);

            CurrentAnimations = asset.Animations;
            AnimationDelay = 1 / asset.FramesPerSecond;

            GameObject.Destroy(asset.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<WoodBreaker>())
            {
                IsBeignHit = true;
            }
        }
    }
}