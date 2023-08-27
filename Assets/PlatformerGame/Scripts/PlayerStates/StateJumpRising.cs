using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpRising : IState
    {
        private Player Model;

        public StateJumpRising(Player model)
        {
            Model = model;
        }

        public void HealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0)
        {
            Model.Timer = time;

            Model.StandUp();
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            Model.DirectionCheck();

            Model.Timer -= Time.fixedDeltaTime;

            // Push Down
            if (Model.Vertical < 0)
            {
                Model.PushDown();
            }

            // Horizontal movement, controllable jump
            if (Model.Horizontal != 0)
            {
                Model.Walk();
            }

            // Check MovingPlatform again after cooldown
            if (Model.Timer <= 0)
            {
                Model.Grounded(LayerMasks.Platforms);
            }

            // State Walk, while in jump on a steep slope  "Solid"
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Slope))
            {
                Model.Animations.Walk();
                Model.SetState(Model.StateWalk);
            }

            // State Jump Falling
            if (Model.DeltaY < 0)
            {
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling);
            }

            // Attack Checks. Animations could be different, but they are not ))
            if (Model.IsAxeAttack())
            {
                Model.ShootAxe();
                Model.SetState(Model.StateJumpRisingAttack, Model.Animations.AirAttack());
            }

            if (Model.IsKnifeAttack())
            {
                Model.ShootKnife();
                Model.SetState(Model.StateJumpRisingAttack, Model.Animations.AirAttack());
            }

            if (Model.IsHolyWaterAttack())
            {
                Model.ShootHolyWater();
                Model.SetState(Model.StateJumpRisingAttack, Model.Animations.AirAttack());
            }
        }

        public void Update()
        {
            Model.GetInput();
        }
    }
}