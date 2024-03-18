using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ToadLaunch : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Sprites;

        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;
        private IResourceManager ResourceManager;

        private float Timer;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Renderer = GetComponent<SpriteRenderer>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            Timer = 3f;
        }

        public void Launch(Vector2 startPosition)
        {
            int choice = UnityEngine.Random.Range(0, 4);
            Renderer.sprite = Sprites[choice];

            var position = startPosition;
            position.x += UnityEngine.Random.Range(-10f, 10f);
            transform.position = position;

            var direction = UnityEngine.Random.Range(3f, 8f);
            var chance = UnityEngine.Random.value;
            if (chance <= 0.5f) direction *= -1;

            if (direction <= 0f) Renderer.flipX = true;
            else Renderer.flipX = false;

            Rigidbody.AddForce(new Vector2(direction, UnityEngine.Random.Range(8f, 15f)), ForceMode2D.Impulse);

            //Splash effect
            var instance = ResourceManager.GetFromPool(GFXs.BlueSplash);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.position = transform.position;
            //instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddMain(instance.gameObject);

        }

        private void FixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            gameObject.SetActive(false);
        }
    }
}