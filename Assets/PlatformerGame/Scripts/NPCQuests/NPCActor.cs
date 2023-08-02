using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class NPCActor : MonoBehaviour, INPCActor
    {
        [SerializeField]
        private Transform SpawnPoint;

        [SerializeField]
        private GameObject Body;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private float AnimationDelay = 0.5f;

        [SerializeField]
        private float AppearTime = 0f;

        [SerializeField]
        private bool IsVisible;

        [SerializeField]
        private bool IsWatchingPlayer;

        private int SpriteIndex;
        private float Timer;
        private bool Inside;
        private IPlayer Player;

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
            Renderer.sprite = Sprites[0];
            CurrentState = Setup;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Inside = true;
            }
        }

        public void WatchPlayer(bool watch)
        {
            IsWatchingPlayer = watch;
        }

        public void HideNPC()
        {
            Body.SetActive(false);
            IsVisible = false;
        }

        private void Setup()
        {
            if (IsVisible)
            {
                Body.SetActive(true);
                Inside = true; //permanent transition to Idle
                Body.transform.position = SpawnPoint.position;
            }

            if (!IsVisible)
            {
                Body.SetActive(false);
            }

            Timer = AppearTime;
            CurrentState = Appear;
        }

        private void Appear()
        {
            if (Inside && !IsVisible)
            {
                Body.SetActive(true);
                IsVisible = true;
                Body.transform.position = SpawnPoint.position;
            }

            if (Inside)
            {
                Timer -= Time.fixedDeltaTime;
                if (Timer <= 0)
                {
                    Timer = 0f;
                    CurrentState = Idle;
                }
            }
        }

        private void Idle()
        {
            Timer += Time.fixedDeltaTime;

            if (Timer >= AnimationDelay)
            {
                Timer -= AnimationDelay;

                if (SpriteIndex == Sprites.Count - 1)
                {
                    SpriteIndex = 0;
                }
                else if (SpriteIndex < Sprites.Count - 1)
                {
                    SpriteIndex++;
                }

                Renderer.sprite = Sprites[SpriteIndex];
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
    }
}