using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FallingShard : MonoBehaviour
    {
        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;
        private Vector3 OldScale;

        private float Torque;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
            OldScale = transform.localScale;
        }

        private void OnEnable()
        {
            Torque = Random.Range(100f, 250f);
            Torque *= Random.Range(0f, 1f) < 0.5f ? 1f : -1f;

            var newScale = OldScale;
            var ratio = Random.Range(0.5f, 1.2f);
            newScale *= ratio;

            transform.localScale = newScale;
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