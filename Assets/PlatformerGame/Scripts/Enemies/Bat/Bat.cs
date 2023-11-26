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
        private SimpleAnimation Animation;

        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;

        private float DirectionX = 1f;
        private float OriginY;
        private float Speed;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Animation = GetComponent<SimpleAnimation>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Health = GetComponent<Health>();

            Health.Killed += OnKilled;
        }

        private void OnEnable()
        {
            
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
                Animation.SetFlipX(false);
            } 
            if (DirectionX == -1f)
            {
                Animation.SetFlipX(true);
            }

            transform.position = startPosition;
            OriginY = Rigidbody.velocity.y;
            Speed = speed;
        }


        private void FixedUpdate()
        {
            float offsetY = Mathf.Sin(Time.time * 4) * 1.5f;
            Rigidbody.velocity = new Vector2(Speed * DirectionX * Time.fixedDeltaTime, OriginY + offsetY);
        }

        private void OnKilled()
        {
            Killed();

            AudioManager.PlaySound(ESounds.Slash2);

            var collider = gameObject.GetComponent<Collider2D>();
            var newPosition = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
            var instance = ResourceManager.GetFromPool(GFXs.BloodBlast);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Main, false);
            dynamics.AddMain(instance.gameObject);
            instance.GetComponent<BloodBlast>().Initiate(newPosition, - Rigidbody.velocity.x);

            gameObject.SetActive(false);
        }
    }
}