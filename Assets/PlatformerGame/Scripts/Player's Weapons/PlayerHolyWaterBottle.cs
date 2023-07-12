using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerHolyWaterBottle : MonoBehaviour
    {
        public delegate void DisappearDelegate(Transform transf);
        public event DisappearDelegate Disappear;

        private SpriteRenderer Renderer;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnDisable()
        {
            Disappear?.Invoke(gameObject.transform);
            Disappear = null;
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