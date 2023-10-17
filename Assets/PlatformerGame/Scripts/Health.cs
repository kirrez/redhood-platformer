using UnityEngine;
using System;

namespace Platformer
{
    public class Health : MonoBehaviour
    {
        public Action Killed = () => { };
        public Action HealthChanged = () => { };

        public float DamageDirection { get; set; }

        [SerializeField]
        private int MaxHitPoints;
        private int CurrentHitPoints;

        [SerializeField]
        private int Type; // 0-Player, 1-Enemy, etc

        [SerializeField]
        private float DamageCooldown;
        private float DamageTimer;
        private bool IsDamageable;
        private bool IsDamageReceived;

        private void OnEnable()
        {
            RefillHealth();
            DamageTimer = 0f;
            IsDamageReceived = false;
        }

        private void FixedUpdate()
        {
            if (DamageTimer <= 0) return;

            DamageTimer -= Time.fixedDeltaTime;
        }

        public int GetHitPoints
        {
            get { return CurrentHitPoints; }
        }

        public void SetMaxLives(int lives)
        {
            MaxHitPoints = lives;
        }

        public int GetCharacterType()
        {
            return Type;
        }

        public void RefillHealth()
        {
            CurrentHitPoints = MaxHitPoints;
            IsDamageable = true;
        }

        public void ResetDamageCooldown()
        {
            DamageTimer = DamageCooldown;
        }

        public bool DamageReceived()
        {
            return IsDamageReceived;
        }

        public void ReceiveDamage(int damage, float direction)
        {
            if (IsDamageable == false) return;

            DamageDirection = direction;

            if (DamageTimer <= 0)
            {
                IsDamageReceived = true;
                ResetDamageCooldown();
                CurrentHitPoints -= damage;
                if (CurrentHitPoints > 0)
                {
                    HealthChanged?.Invoke();
                }
                else if (CurrentHitPoints <= 0)
                {
                    CurrentHitPoints = 0;
                    HealthChanged?.Invoke();
                    IsDamageable = false;
                    Killed?.Invoke();
                }
            }
            else
            {
                IsDamageReceived = false;
            }
        }
    }
}