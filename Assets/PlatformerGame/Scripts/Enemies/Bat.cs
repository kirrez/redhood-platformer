using System;
using UnityEngine;
using System.Collections.Generic;
using Platformer.ScriptedAnimator;

namespace Platformer
{
    public class Bat : MonoBehaviour
    {
        public Action Killed = () => { };

        private Health Health;
        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;
        private IResourceManager ResourceManager;

        private List<Sprite> CurrentAnimations;
        private float AnimationDelay = 0.1f;
        private int SpriteIndex;
        private float Timer;

        private float DirectionX = 1f;
        private float Speed;
        private float OriginY;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Health = GetComponent<Health>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
            Health.Killed += OnKilled;

            LoadSprites();
        }

        private void OnEnable()
        {
            Timer = 0f;
            SpriteIndex = 0;
        }

        private void OnDisable()
        {
            Killed();
        }

        public void Initiate(float direction, Vector2 startPosition, float speed = 300f)
        {
            DirectionX = direction;
            if (DirectionX == 1f)
            {
                Renderer.flipX = false;
            } 
            if (DirectionX == -1f)
            {
                Renderer.flipX = true;
            }

            transform.position = startPosition;
            OriginY = Rigidbody.velocity.y;
            Speed = speed;
        }


        private void FixedUpdate()
        {
            float offsetY = Mathf.Sin(Time.time * 4) * 1.5f;
            Rigidbody.velocity = new Vector2(Speed * DirectionX * Time.fixedDeltaTime, OriginY + offsetY);

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

        private void OnKilled()
        {
            bool direction = false;

            Killed(); // BatSpawner listens to this event

            var collider = gameObject.GetComponent<Collider2D>();
            var newPosition = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
            var instance = ResourceManager.GetFromPool(GFXs.BloodBlast);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance.gameObject);

            if (Rigidbody.velocity.x > 0)
            {
                direction = true;
            }
            if (Rigidbody.velocity.x < 0)
            {
                direction = false;
            }

            instance.GetComponent<BloodBlast>().Initiate(newPosition, direction);


            gameObject.SetActive(false);
        }

        private void LoadSprites()
        {
            var resourceManager = CompositionRoot.GetResourceManager();

            ScriptedAnimation asset = resourceManager.CreatePrefab<ScriptedAnimation, EGFXAnimations>(EGFXAnimations.BatFlying);
            CurrentAnimations = asset.Animations;
            AnimationDelay = 1 / asset.FramesPerSecond;

            GameObject.Destroy(asset.gameObject);
        }
    }
}