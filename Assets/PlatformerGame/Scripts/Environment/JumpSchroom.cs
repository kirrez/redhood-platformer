using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class JumpSchroom : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private Collider2D Trigger;

        [SerializeField]
        private Vector2 JumpDirection;

        private float Timer;
        private float Delay = 0.5f; //delay between jumps

        private IPlayer Player;
        private IAudioManager AudioManager; 

        protected delegate void State();
        protected State CurrentState = () => { };

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            AudioManager = CompositionRoot.GetAudioManager();
        }

        private void OnEnable()
        {
            CurrentState = StateWaitPlayer;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private bool CheckCollision()
        {
            var Bounds = Player.Body.GetComponent<Collider2D>().bounds;
            var newBounds = Bounds.center;
            newBounds.x -= Bounds.extents.x;
            if (Trigger.bounds.Contains(newBounds)) return true;

            newBounds = Bounds.center;
            newBounds.x += Bounds.extents.x;
            if (Trigger.bounds.Contains(newBounds)) return true;

            newBounds = Bounds.center;
            newBounds.y -= Bounds.extents.y;
            if (Trigger.bounds.Contains(newBounds)) return true;

            newBounds = Bounds.center;
            newBounds.y += Bounds.extents.y;
            if (Trigger.bounds.Contains(newBounds)) return true;

            return false;
        }

        private void StateWaitPlayer()
        {
            if (CheckCollision())
            {
                var newVelocity = Player.Body.velocity;
                newVelocity.y = 0f;

                Player.Body.velocity = newVelocity;
                Player.Body.AddForce(JumpDirection, ForceMode2D.Impulse);
                AudioManager.PlaySound(ESounds.Quack1);
                Renderer.sprite = Sprites[1];
                Timer = Delay;

                CurrentState = StateCooldown;
            }
        }

        private void StateCooldown()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer <= 0)
            {
                Renderer.sprite = Sprites[0];

                CurrentState = StateWaitPlayer;
            }
        }
    }
}