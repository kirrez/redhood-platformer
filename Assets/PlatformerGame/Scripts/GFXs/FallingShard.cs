using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FallingShard : MonoBehaviour
    {
        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;

        private float Torque;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            Torque = Random.Range(100f, 250f);
            Torque *= Random.Range(0f, 1f) < 0.5f ? 1f : -1f;

            var scale = Random.Range(0.5f, 1f);
            transform.localScale = new Vector2(scale, scale);
        }
        private void FixedUpdate()
        {
            Rigidbody.AddTorque(Torque * Time.fixedDeltaTime);
        }

        public void Initiate(Vector2 newPosition, float direction = 1)
        {
            transform.position = newPosition;
            Renderer.flipX = direction < 0 ? true : false;
        }
    }
}