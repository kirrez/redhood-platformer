using System.Collections;
using System;
using UnityEngine;

namespace Platformer
{
    public class BearCub : MonoBehaviour
    {
        public Action Killed = () => { };

        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private Collider2D Collider;

        [SerializeField]
        private Collider2D DamageTrigger;

        [SerializeField]
        private Health Health;

        private BearCubAnimator Animator;

        delegate void State();
        State CurrentState = () => { };

        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private float Timer;
        private float JumpCooldown;
        private float GroundCheckCooldown;
        private float DirectionX;
        private float HorizontalSpeed = 125f;
        private bool IsRoamingState;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Animator = GetComponent<BearCubAnimator>();

            Health.Killed += OnKilled;
            Health.HealthChanged += OnHealthChanged;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;
        }
        public void Initiate(Vector2 startPosition)
        {
            Body.transform.position = startPosition;
            DirectionX = 1f;
            Animator.PlayWalk();

            if (DirectionX == 1f) Animator.SetFlip(false);
            if (DirectionX == -1f) Animator.SetFlip(true);
            
            SetMask(LayerNames.EnemySolid);
            UnfreezeBody();
            DamageTrigger.enabled = true;

            IsRoamingState = true;
            CurrentState = StateRoaming;
        }

        private void OnDisable()
        {
            IsRoamingState = true;
            Killed();
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void SetMask(string mask)
        {
            Body.gameObject.layer = LayerMask.NameToLayer(mask);
        }

        private void FreezeBody()
        {
            Body.isKinematic = true;
        }

        private void UnfreezeBody()
        {
            Body.isKinematic = false;
        }

        private void OnHealthChanged()
        {
            Animator.StartBlinking();
            SetMask(LayerNames.EnemyInactive);
            DamageTrigger.enabled = false;

            AudioManager.PlaySound(ESounds.EnemyDamage2);
        }

        private void OnDamageCooldownExpired()
        {
            if (Health.GetHitPoints > 0)
            {
                SetMask(LayerNames.EnemySolid);
                DamageTrigger.enabled = true;
            }
        }

        private void OnKilled()
        {
            IsRoamingState = false;
            CurrentState = StateDying;
        }

        public void TryJump(float force, float cooldown)
        {
            if (IsRoamingState == true && JumpCooldown <= 0f)
            {
                JumpCooldown = cooldown;
                GroundCheckCooldown = 0.5f;
                IsRoamingState = false;

                Body.AddForce(new Vector2(Body.velocity.x, force));
                Animator.PlayJump();
                //AudioManager.

                CurrentState = StateJump;
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

        private bool CheckGround(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var boxSize = new Vector2(Collider.bounds.size.x *0.8f, 0.1f);
            float distance = 0.1f;

            RaycastHit2D GroundHit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, distance, mask);

            return GroundHit.collider != null;
        }

        private void StateDying()
        {
            var newPosition = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameEffect);
            DynamicsContainer.AddMain(instance);

            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(2f, 2f));

            Animator.StopBlinking();
            Timer = Animator.PlayDying();

            SetMask(LayerNames.EnemyInactive);
            Body.velocity = Vector2.zero;
            FreezeBody();
            DamageTrigger.enabled = false;

            AudioManager.PlaySound(ESounds.EnemyDying5);
            CurrentState = StateDyingFinal;
        }

        private void StateDyingFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Killed();
            UnfreezeBody();

            gameObject.SetActive(false);
        }

        private void StateRoaming()
        {
            Body.velocity = new Vector2(DirectionX * Time.fixedDeltaTime * HorizontalSpeed, Body.velocity.y);

            if (JumpCooldown > 0f)
            {
                JumpCooldown -= Time.fixedDeltaTime;
            }

            // Walls
            if (CheckWall(LayerMasks.Ground + LayerMasks.EnemyBorder))
            {
                DirectionX *= -1f;

                if (DirectionX == 1f) Animator.SetFlip(false);
                if (DirectionX == -1f) Animator.SetFlip(true);
            }
        }

        private void StateJump()
        {
            Body.velocity = new Vector2(DirectionX * Time.fixedDeltaTime * HorizontalSpeed, Body.velocity.y);

            if (JumpCooldown > 0f)
            {
                JumpCooldown -= Time.fixedDeltaTime;
            }

            // Walls
            if (CheckWall(LayerMasks.Ground + LayerMasks.EnemyBorder))
            {
                DirectionX *= -1f;

                if (DirectionX == 1f) Animator.SetFlip(false);
                if (DirectionX == -1f) Animator.SetFlip(true);
            }

            // a little gap before checking ground in jump
            GroundCheckCooldown -= Time.fixedDeltaTime;
            if (GroundCheckCooldown > 0) return;

            if (CheckGround(LayerMasks.Ground) == true)
            {
                IsRoamingState = true;
                Animator.PlayWalk();
                CurrentState = StateRoaming;
            }
        }
    }
}