using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public abstract class BaseState
    {
        protected IPlayer Model;

        protected float Timer;

        public virtual void OnEnable(float time = 0f)
        {
            Timer = time;
            Model.Health.HealthChanged = null;
            Model.Health.HealthChanged += OnHealthChanged;
        }

        protected virtual void OnHealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(EPlayerStates.DamageTaken);
        }

        public virtual void Update()
        {
            Model.GetInput();
        }

        public virtual void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();
        }

    }
}