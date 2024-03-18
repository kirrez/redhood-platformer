using UnityEngine;
using System;

namespace Platformer
{
    public class MummyWalker : Undead
    {
        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private Collider2D Collider;

        [SerializeField]
        private Collider2D DamageTrigger;

        private MummyAnimator Animator;

        private delegate void State();
        State CurrentState = () => { };

        private IPlayer Player;
        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private float FreezeTimer;
        private bool IsFrozen;

        private float Timer;
        private float DirectionX = 1f;
        private float WalkingForce = 50f; // have to change

        public void Initiate(Vector2 newPosition, float direction)
        {
            Body.transform.position = newPosition;
            DirectionX = direction;
            //change walk direction, based on "direction"

            DamageTrigger.enabled = true;

            Animator.PlayWalking();
            Animator.Begin();

            if (direction > 0) Animator.SetFlip(false);
            if (direction < 0) Animator.SetFlip(true);

            CurrentState = StateWalking;
        }

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();

            Animator = GetComponent<MummyAnimator>();

            Freezing -= OnStartFreezing;
            Freezing += OnStartFreezing;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void OnStartFreezing(float duration)
        {
            if (IsFrozen == false)
            {
                IsFrozen = true;
                DamageTrigger.enabled = false;
                FreezeTimer = duration;
                Animator.StartFreeze(duration);
                //OldVelocity = Body.velocity;
                Body.velocity = Vector2.zero;
            }
        }

        private bool CheckWall(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x + Collider.bounds.extents.x * DirectionX, Collider.bounds.center.y);
            var boxSize = new Vector2(0.2f, 1.6f);
            float distance = 0.1f; // Magic number, empirical
            RaycastHit2D WallHit = Physics2D.BoxCast(origin, boxSize, 0f, new Vector2(1f * DirectionX, 0f), distance, mask);

            return WallHit.collider != null;
        }

        private void StateWalking()
        {
            // Freeze part in every state
            if (IsFrozen == true)
            {
                FreezeTimer -= Time.fixedDeltaTime;

                if (FreezeTimer > 0) return;

                if (FreezeTimer <= 0)
                {
                    IsFrozen = false;
                    DamageTrigger.enabled = true;
                    //Body.velocity = OldVelocity;
                }
            }
            //

            // Walls
            if (CheckWall(LayerMasks.Ground + LayerMasks.EnemyBorder))
            {
                DirectionX *= -1f;

                if (DirectionX == 1f) Animator.SetFlip(false);
                if (DirectionX == -1f) Animator.SetFlip(true);
            }

            //Body.AddForce(new Vector2(DirectionX * WalkingForce * Time.fixedDeltaTime, 0f));
            Body.velocity = new Vector2(DirectionX * WalkingForce * Time.fixedDeltaTime, Body.velocity.y);
        }
    }
}