using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpRisingAttack : IState
    {
        private Player Model;

        public StateJumpRisingAttack(Player model)
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

            // State Idle, animation interrupted
            if (Model.Horizontal == 0 && Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(false);
                Model.Animations.Idle();
                Model.SetState(Model.StateIdle);
            }

            // State Walk, animation interrupted
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(false);
                Model.Animations.Walk();
                Model.SetState(Model.StateWalk);
            }

            // State Jump Rising, long jump
            if (Model.DeltaY > 0 && Model.Timer <= 0)
            {
                Model.Animations.JumpRising();
                Model.SetState(Model.StateJumpRising);
            }

            // State JumpFalling
            if (Model.DeltaY < 0 && Model.Timer <= 0)
            {
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling);
            }

            // we can shoot any weapon if it's timer = 0
            if (Model.IsAxeAttack()) Model.ShootAxe();

            if (Model.IsKnifeAttack()) Model.ShootKnife();

            if (Model.IsHolyWaterAttack()) Model.ShootHolyWater();
        }
    }
}