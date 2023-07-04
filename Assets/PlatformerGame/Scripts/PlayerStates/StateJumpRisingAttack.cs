using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpRisingAttack : BaseState
    {
        public StateJumpRisingAttack(IPlayer model)
        {
            Model = model;
        }

        public override void OnEnable(float time = 0)
        {
            base.OnEnable(time);
            Model.UpdateStateName("Jump Rising Attack");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Model.DirectionCheck();

            Timer -= Time.fixedDeltaTime;

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
                Model.SetState(EPlayerStates.Idle);
            }

            // State Walk, animation interrupted
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(false);
                Model.Animations.Walk();
                Model.SetState(EPlayerStates.Walk);
            }

            // State Jump Rising, long jump
            if (Model.DeltaY > 0 && Timer <= 0)
            {
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // State JumpFalling
            if (Model.DeltaY < 0 && Timer <= 0)
            {
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling);
            }

            // we can shoot any weapon if it's timer = 0
            if (Model.IsKnifeAttack()) Model.ShootKnife();

            if (Model.IsAxeAttack()) Model.ShootAxe();

            if (Model.IsHolyWaterAttack()) Model.ShootHolyWater();
        }
    }
}