using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class NPCWanderer : MonoBehaviour
    {
        [SerializeField]
        private GameObject Body;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private List<Sprite> IdleSprites;

        [SerializeField]
        private float IdleAnimationDelay = 0.5f;

        [SerializeField]
        private List<Sprite> WanderSprites;

        [SerializeField]
        private float WanderAnimationDelay = 0.5f;

        [SerializeField]
        private List<Transform> WayPoints;

        [SerializeField]
        private float Speed = 100f;

        [SerializeField]
        private bool AlterDirection;

        private int SpriteIndex;
        //private IPlayer Player;
        private float Direction = 1f;
        private float Timer;

        delegate void StateMethod();
        StateMethod CurrentState = () => { };

        private void OnEnable()
        {
            Timer = 0f;
            SpriteIndex = 0;
            Renderer.sprite = IdleSprites[0];
            if (AlterDirection)
            {
                Direction *= -1f;
            }
            SetDirection();
            CurrentState = WanderState;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void SetDirection()
        {
            if (Direction == 1)
            {
                Renderer.flipX = false;
            }

            if (Direction == -1)
            {
                Renderer.flipX = true;
            }
        }

        private void PlayAnimation(List<Sprite> sprites, float delay)
        {
            Timer += Time.fixedDeltaTime;

            if (Timer >= delay)
            {
                Timer -= delay;

                if (SpriteIndex == sprites.Count - 1)
                {
                    SpriteIndex = 0;
                }
                else if (SpriteIndex < sprites.Count - 1)
                {
                    SpriteIndex++;
                }

                Renderer.sprite = sprites[SpriteIndex];
            }
        }

        private void WanderState()
        {
            PlayAnimation(WanderSprites, WanderAnimationDelay);

        }

        private void IdleState()
        {

        }
    }
}

