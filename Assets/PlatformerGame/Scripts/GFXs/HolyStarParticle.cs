using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HolyStarParticle : MonoBehaviour
    {
        [SerializeField]
        private float Amplitude;

        private void OnEnable()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(Random.insideUnitCircle.normalized * Amplitude, ForceMode2D.Impulse);
            rigidbody.angularVelocity = Random.Range(100f, 200f);
        }

        public void Initiate(Vector2 newPosition)
        {
            transform.position = newPosition;
        }
    }
}