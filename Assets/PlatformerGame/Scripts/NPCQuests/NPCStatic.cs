using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class NPCStatic : MonoBehaviour
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
        private bool IsWatchingPlayer;

        private int SpriteIndex;
        private IPlayer Player;
        private float Timer;

        delegate void StateMethod();
        StateMethod CurrentState = () => { };

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            Timer = 0f;
            SpriteIndex = 0;
            Renderer.sprite = IdleSprites[0];
            CurrentState = IdleState;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void IdleState()
        {
            Timer += Time.fixedDeltaTime;

            if (Timer >= IdleAnimationDelay)
            {
                Timer -= IdleAnimationDelay;

                if (SpriteIndex == IdleSprites.Count - 1)
                {
                    SpriteIndex = 0;
                }
                else if (SpriteIndex < IdleSprites.Count - 1)
                {
                    SpriteIndex++;
                }

                Renderer.sprite = IdleSprites[SpriteIndex];
            }

            if (IsWatchingPlayer)
            {
                var direction = Player.Position.x - transform.position.x;

                if (direction <= 0)
                {
                    Renderer.flipX = true;
                }

                if (direction > 0)
                {
                    Renderer.flipX = false;
                }
            }
        }

        public void WatchPlayer(bool watch)
        {
            IsWatchingPlayer = watch;
        }

        public void HideNPC()
        {
            Body.SetActive(false);
        }
    }
}