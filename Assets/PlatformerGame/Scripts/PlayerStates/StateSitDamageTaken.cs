using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateSitDamageTaken : IState
    {
        private int HitPoints;

        private Player Model;

        public StateSitDamageTaken(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            // no player control input!
        }

        public void HealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0f)
        {
            Model.Timer = time;
            Model.ResetVelocity();

            HitPoints = Model.Health.GetHitPoints;
        }

        public void FixedUpdate()
        {
            // no base

            Model.Timer -= Time.fixedDeltaTime;

            if (Model.Timer <= 0)
            {
                // Dying, if HP <= 0
                if (HitPoints <= 0)
                {
                    Model.Animations.Dying();
                    Model.InactivateCollider(true);
                    Model.SetState(Model.StateDying, Model.Animations.Dying() + Model.DeathShockTime);
                }
                else
                {
                    Model.Animations.Sit();
                    Model.SetState(Model.StateSit);
                }
            }
        }

        public void OnHealthChanged()
        {
            // doing just nothing )
        }
    }
}