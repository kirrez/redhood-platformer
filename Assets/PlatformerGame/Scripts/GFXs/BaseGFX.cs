using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BaseGFX : MonoBehaviour, IBaseGFX
    {
        protected SpriteRenderer Renderer;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }
        public virtual void Initiate(Vector2 newPosition, float direction = 1f)
        {
            transform.position = newPosition;
            Renderer.flipX = direction < 0 ? true : false;
        }
    }
}