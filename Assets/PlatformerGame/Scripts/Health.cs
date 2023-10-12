using UnityEngine;
using System;

namespace Platformer
{
    // this class should be largely refactored, should become not a monobeh etc...
    public class Health : MonoBehaviour
    {
        public Action Killed = () => { };
        public Action HealthChanged = () => { };
        //now for frog only
        public Action DamageCooldownExpired = () => { };

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

        private void OnEnable()
        {
            RefillHealth();
            DamageTimer = 0f;
        }

        private void FixedUpdate()
        {
            // DamageCooldownExpired will be invoked once after each resetting of DamageTimer
            if (DamageTimer > 0)
            {
                DamageTimer -= Time.deltaTime;
                if (DamageTimer <= 0)
                {
                    DamageCooldownExpired?.Invoke();
                }
            }
        }

        public int GetHitPoints
        {
            get { return CurrentHitPoints; }
        }


        //for Player
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

        public void ReceiveDamage(int damage, float direction)
        {
            if (!IsDamageable) return;

            DamageDirection = direction;

            if (DamageTimer <= 0)
            {
                ResetDamageCooldown();
                CurrentHitPoints -= damage;
                if (CurrentHitPoints > 0)
                {
                    HealthChanged?.Invoke();
                }
                else if (CurrentHitPoints <= 0)
                {
                    //now i need only HealthChanged in Player's states ))
                    CurrentHitPoints = 0;
                    HealthChanged?.Invoke();
                    IsDamageable = false;
                    Killed?.Invoke();
                }
            }
        }
    }
}