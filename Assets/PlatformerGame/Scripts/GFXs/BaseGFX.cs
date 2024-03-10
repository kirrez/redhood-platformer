using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class BaseGFX : MonoBehaviour, IBaseGFX
    {
        public Action Disappear;
        protected SpriteRenderer Renderer;

        protected IDynamicsContainer DynamicsContainer;
        protected IResourceManager ResourceManager;
        protected IAudioManager AudioManager;

        protected virtual void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();

            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();

            Disappear -= OnDisappear;
            Disappear += OnDisappear;
        }
        public virtual void Initiate(Vector2 newPosition, float direction = 1f)
        {
            transform.position = newPosition;
            Renderer.flipX = direction < 0 ? true : false;
        }

        protected virtual void OnDisappear()
        {
            gameObject.SetActive(false);
        }
    }
}