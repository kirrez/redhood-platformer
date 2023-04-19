using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateSitDamageTaken : BaseState
    {
        private int HitPoints;
        public StateSitDamageTaken(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0f)
        {
            Timer = time;
            Model.UpdateStateName("Sit Damage Taken");
            Model.ResetVelocity();

            HitPoints = Model.Health.GetHitPoints;
        }

        public override void OnUpdate()
        {
            // no input!
        }

        public override void OnFixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                // Dying, if HP <= 0
                if (HitPoints <= 0)
                {
                    Model.Animations.Dying();
                    Model.InactivateCollider();
                    Model.SetState(EPlayerStates.Dying, Model.Animations.Dying() + Model.DeathShockTime);
                }
                else
                {
                    Model.Animations.Sit();
                    Model.SetState(EPlayerStates.Sit);
                }
            }
        }

        protected override void OnHealthChanged()
        {
            // doing just nothing )
        }
    }
}