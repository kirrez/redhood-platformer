using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SimpleAnimation : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private float AnimationDelay = 0.25f;

        [SerializeField]
        private bool NoLoop;

        private int SpriteIndex;
        private float Timer;

        private SpriteRenderer Renderer;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            SpriteIndex = 0;
            Timer = 0f;
        }

        private void FixedUpdate()
        {
            Timer += Time.fixedDeltaTime;

            if (Timer >= AnimationDelay)
            {
                Timer -= AnimationDelay;

                if (SpriteIndex == Sprites.Count - 1)
                {
                    if (NoLoop == false) SpriteIndex = 0;
                }
                else if (SpriteIndex < Sprites.Count - 1)
                {
                    SpriteIndex++;
                }

                Renderer.sprite = Sprites[SpriteIndex];
            }
        }

        public void SetFlipX(bool flip)
        {
            Renderer.flipX = flip;
        }
    }
}