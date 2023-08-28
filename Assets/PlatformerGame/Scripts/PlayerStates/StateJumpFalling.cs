using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpFalling : IState
    {
        private Player Model;

        public StateJumpFalling(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            Model.GetInput();
        }

        public void HealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0)
        {
            Model.Timer = time;
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            Model.UpdateFacing();

            //StandUp happens immediately or after a delay, if something is passed in Activate()
            if (Model.Timer >= 0)
            {
                Model.Timer -= Time.fixedDeltaTime;

                if (Model.Timer < 0)
                {
                    Model.StandUp();
                }
            }

            // Horizontal movement, should change to some more smooth logic for controllable falling
            if (Model.Horizontal != 0)
            {
                Model.Walk();
            }

            // Trying to stick..
            if (Model.Grounded(LayerMasks.Platforms))
            {
                Model.StickToPlatform();
            }


            // State Idle
            if (Model.Horizontal == 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.ResetVelocity();
                //Model.UpdateInAir(false);
                Model.Animations.Idle();
                Model.SetState(Model.StateIdle);
            }

            // State Walk
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(false);
                Model.Animations.Walk();
                Model.SetState(Model.StateWalk);
            }

            // Attack Checks. Animations could be different, but they are not ))
            if (Model.IsAxeAttack())
            {
                Model.ShootAxe();
                Model.SetState(Model.StateJumpFallingAttack, Model.Animations.AirAttack());
            }

            if (Model.IsKnifeAttack())
            {
                Model.ShootKnife();
                Model.SetState(Model.StateJumpFallingAttack, Model.Animations.AirAttack());
            }

            if (Model.IsHolyWaterAttack())
            {
                Model.ShootHolyWater();
                Model.SetState(Model.StateJumpFallingAttack, Model.Animations.AirAttack());
            }
        }
    }
}