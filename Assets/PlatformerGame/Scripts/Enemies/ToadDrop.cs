using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ToadDrop : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Sprites;

        private Health Health;
        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;
        private IResourceManager ResourceManager;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Renderer = GetComponent<SpriteRenderer>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Health = GetComponent<Health>();
            Health.Killed += OnKilled;
        }

        private void FixedUpdate()
        {
            
        }

        private void OnKilled()
        {
            var collider = gameObject.GetComponent<Collider2D>();
            var newPosition = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
            var instance = ResourceManager.GetFromPool(GFXs.BloodBlast);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance.gameObject);
            instance.GetComponent<BloodBlast>().Initiate(newPosition, Random.Range(-1f, 1f));

            gameObject.SetActive(false);
        }

        public void Initiate(Vector2 startPosition)
        {
            int choice = UnityEngine.Random.Range(0, 4);
            Renderer.sprite = Sprites[choice];

            var position = startPosition;
            position.x += UnityEngine.Random.Range(-13f, 13f);
            transform.position = position;


            Rigidbody.AddForce(Vector2.down * 0.5f, ForceMode2D.Impulse);

            var rotation = 5f;
            var chance = UnityEngine.Random.value;
            if (chance <= 0.5f) rotation *= -1;

            Rigidbody.AddTorque(rotation, ForceMode2D.Impulse);
        }
    }
}