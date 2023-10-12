using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private Collider2D Collider;

        private int SpriteIndex;
        private float Timer;
        private bool IsActive;

        private void Awake()
        {
            SpriteIndex = 0;
        }

        private void FixedUpdate()
        {
            if (!IsActive) return;

            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 0.06f;
            SpriteIndex++;
            if (SpriteIndex > Sprites.Count - 1) SpriteIndex = 0;
            Renderer.sprite = Sprites[SpriteIndex];
        }

        public void Show()
        {
            IsActive = true;

            Renderer.enabled = true;
            Collider.enabled = true;
        }

        public void Hide()
        {
            IsActive = false;

            Renderer.enabled = false;
            Collider.enabled = false;
        }
    }
}