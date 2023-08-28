using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateDamageTaken : IState
    {
        private int HitPoints;

        private Player Model;

        public StateDamageTaken(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            //no player control

            //Model.GetInput();
        }

        public void HealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0f)
        {
            //sitDamageCheck
            if (Model.Ceiled(LayerMasks.Solid) && Model.Grounded(LayerMasks.Solid))
            {
                Model.SetState(Model.StateSitDamageTaken, Model.Animations.SitDamageTaken());
            }
            else
            {
                Model.Timer = Model.Animations.DamageTaken();
                HitPoints = Model.Health.GetHitPoints;

                Model.StandUp();
                Model.ResetVelocity();
                Model.DamagePushBack();
            }
        }

        public void FixedUpdate()
        {
            // no base

            Model.Timer -= Time.fixedDeltaTime;

            if (Model.Timer <= 0)
            {
                // Idle
                if (Model.Grounded(LayerMasks.Walkable))
                {
                    // Dying, if HP <= 0
                    Model.Horizontal *= -1;
                    Model.UpdateFacing();

                    if (HitPoints <= 0)
                    {
                        Model.Animations.Dying();
                        Model.InactivateCollider(true);
                        Model.SetState(Model.StateDying, Model.Animations.Dying() + Model.DeathShockTime);
                    }
                    else
                    {
                        Model.Animations.Idle();
                        Model.SetState(Model.StateIdle);
                    }
                }
            }
        }

        public void OnHealthChanged()
        {
            // doing just nothing )
        }
    }
}