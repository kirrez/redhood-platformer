using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerKnife : MonoBehaviour
    {
        private SpriteRenderer Renderer;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
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
    }
}