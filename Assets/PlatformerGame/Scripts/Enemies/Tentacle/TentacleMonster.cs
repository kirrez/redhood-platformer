using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer
{
    public class TentacleMonster : MonoBehaviour
    {
        [SerializeField]
        private TentacleAnimator Tentacles;

        [SerializeField]
        private CapAnimator Cap;

        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private PolygonCollider2D BodyCollider; // for transforming collider's form during attack

        [SerializeField]
        private Health Health;

        [SerializeField]
        private PolygonCollider2D DamageTrigger; // also for transforming form during Attack

        [SerializeField]
        private Collider2D DetectionTrigger;

        [SerializeField]
        private float CooldownTime = 0.75f;

        [SerializeField]
        private float ResurrectionTime = 3f;

        [SerializeField]
        private Transform ExpandPosition1;

        [SerializeField]
        private Transform ExpandPosition2;

        [SerializeField]
        private TentacleShardDropper Dropper;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private IPlayer Player;

        private float Timer;
        private float Period;

        private Vector3 StartPosition1;
        private Vector3 StartPosition2;

        private Vector2[] NewPath;

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();

            Health.Killed += OnKilled;
            Health.HealthChanged += OnHealthChanged;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;
        }

        private void OnEnable()
        {
            SetMask(LayerNames.EnemySolid);
            DamageTrigger.enabled = true;
            NewPath = DamageTrigger.GetPath(0);
            StartPosition1 = NewPath[1];
            StartPosition2 = NewPath[2];
            Dropper.gameObject.SetActive(false);
            
            //Animators' first run
            Tentacles.PlayIdle();
            Tentacles.Begin();
            Cap.PlayIdle();
            Cap.Begin();

            CurrentState = StateAwait;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void SetMask(string mask)
        {
            Body.gameObject.layer = LayerMask.NameToLayer(mask);
        }

        private void OnDamageCooldownExpired()
        {
            if (Health.GetHitPoints > 0)
            {
                SetMask(LayerNames.EnemySolid);
            }
        }

        private void OnHealthChanged()
        {
            if (Health.GetHitPoints > 0)
            {
                Tentacles.StartBlinking();
                SetMask(LayerNames.EnemyInactive);
            }

            AudioManager.PlaySound(ESounds.EnemyDamage2);
        }

        private void OnKilled()
        {
            CurrentState = StateStartClosing;
        }

        private void StateAwait()
        {
            if (DetectionTrigger.bounds.Contains(Player.Position))
            {
                Timer = Tentacles.PlayAttack();
                Period = Timer; // for Lerping points
                NewPath[1] = StartPosition1;
                NewPath[2] = StartPosition2;

                //AudioManager
                CurrentState = StateAttacking;
            }
        }

        private void StateAttacking()
        {
            float progress;

            if (Timer > 0)
            {
                Timer -= Time.fixedDeltaTime;

                // transformation of colliders
                if (Timer >= Period / 2)
                {
                    progress = (Period - Timer) * 2;
                }
                else
                {
                    progress = Timer * 2;
                }

                NewPath[1] = Vector2.Lerp(StartPosition1, ExpandPosition1.localPosition, progress);
                NewPath[2] = Vector2.Lerp(StartPosition2, ExpandPosition2.localPosition, progress);

                DamageTrigger.SetPath(0, NewPath);
                BodyCollider.SetPath(0, NewPath);
            }

            if (Timer <= 0)
            {
                Tentacles.PlayIdle();
                Timer = CooldownTime;

                CurrentState = StateAttackCooldown;
            }
        }

        private void StateAttackCooldown()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                CurrentState = StateAwait;
            }
        }

        private void StateStartClosing()
        {
            NewPath[1] = StartPosition1;
            NewPath[2] = StartPosition2;
            DamageTrigger.SetPath(0, NewPath);
            BodyCollider.SetPath(0, NewPath);

            SetMask(LayerNames.EnemyInactive);
            DamageTrigger.enabled = false;

            Tentacles.PlayEmpty();
            Timer = Cap.PlayClosing();

            var newPosition = new Vector2(transform.position.x, transform.position.y - 2f);
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameEffect);
            DynamicsContainer.AddMain(instance);
            instance.GetComponent<DeathFlameEffect>().Initiate(newPosition, new Vector2(2f, 2f));
            AudioManager.PlaySound(ESounds.EnemyDying5);

            for (int i = 0; i < 7; i++)
            {
                var shatter = ResourceManager.GetFromPool(GFXs.TentacleShatter);
                DynamicsContainer.AddMain(shatter);
                newPosition = transform.position;
                newPosition.y += -2f;
                shatter.GetComponent<WoodShatter>().Initiate(newPosition);
            }
            Dropper.gameObject.SetActive(true);

            CurrentState = StateFinishClosing;
        }

        private void StateFinishClosing()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = ResurrectionTime;
                Cap.PlayHybernating();

                CurrentState = StateHybernating;
            }
        }

        private void StateHybernating()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = Cap.PlayOpening();

                CurrentState = StateStartOpening;
            }
        }

        private void StateStartOpening()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = Tentacles.PlayRespawn();
                Cap.PlayIdle();

                CurrentState = StateFinishOpening;
            }
        }

        private void StateFinishOpening()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Tentacles.PlayIdle();
                Health.RefillHealth();
                SetMask(LayerNames.EnemySolid);
                DamageTrigger.enabled = true;

                CurrentState = StateAwait;
            }
        }
    }
}