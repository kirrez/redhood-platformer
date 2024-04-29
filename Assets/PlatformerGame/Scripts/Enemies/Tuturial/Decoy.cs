using UnityEngine;
using System;

namespace Platformer
{
    public class Decoy : MonoBehaviour
    {
        public Action Killed = () => { };

        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private GameObject Block;

        private float Timer;

        private DecoyAnimator Animator;
        private Collider2D Collider;
        private Health Health;

        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();

            Collider = Body.GetComponent<Collider2D>();
            Animator = GetComponent<DecoyAnimator>();
            Health = Body.GetComponent<Health>();

            Health.Killed += OnKilled;
            Health.HealthChanged += OnHealthChanged;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;
        }

        private void OnEnable()
        {
            Animator.PlayIdle();
            Animator.Begin();

            Block.SetActive(true);
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void OnHealthChanged()
        {
            Animator.StartBlinking();
            Body.gameObject.layer = LayerMask.NameToLayer(LayerNames.EnemyInactive);

            AudioManager.PlaySound(ESounds.EnemyDamage2);
        }

        private void OnDamageCooldownExpired()
        {
            if (Health.GetHitPoints > 0)
            {
                Body.gameObject.layer = LayerMask.NameToLayer(LayerNames.EnemySolid);
            }
        }

        private void OnKilled()
        {
            Block.SetActive(false);
            CurrentState = StateDying;
        }

        private void StateDying()
        {
            var newPosition = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameEffect);
            DynamicsContainer.AddMain(instance);

            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(2f, 3f));

            //Animator.PlayDying();
            Animator.StopBlinking();

            Body.gameObject.layer = LayerMask.NameToLayer(LayerNames.EnemyInactive);
            Timer = 1.35f;

            AudioManager.PlaySound(ESounds.EnemyDying5);

            CurrentState = StateDyingFinal;
        }

        private void StateDyingFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Killed();

            gameObject.SetActive(false);
        }
    }
}