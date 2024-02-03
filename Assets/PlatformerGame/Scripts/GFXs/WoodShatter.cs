using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class WoodShatter : BaseGFX
    {
        [SerializeField]
        private float Amplitude = 200f;

        private IDynamicsContainer DynamicsContainer;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
        }

        private void OnEnable()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(new Vector3(Random.Range(-10f, 10f), Random.Range(10f, 25f), 0f) * Amplitude);
            
            var chance = Random.Range(0f, 1f);
            if (chance < 0.5f) rigidbody.angularVelocity = Random.Range(100f, 200f);
            if (chance >= 0.5f) rigidbody.angularVelocity = Random.Range(100f, 200f) * -1f;

            DynamicsContainer.AddMain(this.gameObject);
        }

        public void Initiate(Vector2 newPosition)
        {
            transform.position = newPosition;
        }
    }
}