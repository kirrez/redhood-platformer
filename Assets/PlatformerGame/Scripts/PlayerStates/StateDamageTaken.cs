using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
                Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly

                // direction from Health ))
                Model.Horizontal = Model.Health.DamageDirection;
                Model.UpdateFacing();

                // magic numbers, no need to take out into config.. 2.3f / 1.75f
                Model.Rigidbody.AddForce(new Vector2(Model.HorizontalSpeed / 2.3f * Model.Horizontal, Model.JumpForce / 1.75f));
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