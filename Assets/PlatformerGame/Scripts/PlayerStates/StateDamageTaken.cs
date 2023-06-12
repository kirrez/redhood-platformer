using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateDamageTaken : BaseState
    {
        private int HitPoints;
        public StateDamageTaken(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0f)
        {
            Model.UpdateStateName("Damage Taken");
            Model.Health.HealthChanged = null; //cleaning previous handler

            //sitDamageCheck
            if (Model.Ceiled(LayerMasks.Solid) && Model.Grounded(LayerMasks.Solid))
            {
                Model.SetState(EPlayerStates.SitDamageTaken, Model.Animations.SitDamageTaken());
            }
            else
            {
                Timer = Model.Animations.DamageTaken();
                HitPoints = Model.Health.GetHitPoints;

                Model.StandUp();
                Model.ResetVelocity();
                Model.DamagePushBack();
            }
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
                // Idle
                if (Model.Grounded(LayerMasks.Walkable))
                {
                    // Dying, if HP <= 0
                    Model.Horizontal *= -1;
                    Model.DirectionCheck();

                    if (HitPoints <= 0)
                    {
                        Model.Animations.Dying();
                        Model.InactivateCollider(true);
                        Model.SetState(EPlayerStates.Dying, Model.Animations.Dying() + Model.DeathShockTime);
                    }
                    else
                    {
                        Model.Animations.Idle();
                        Model.SetState(EPlayerStates.Idle);
                    }
                }
            }
        }

        protected override void OnHealthChanged()
        {
            // doing just nothing )
        }
    }
}