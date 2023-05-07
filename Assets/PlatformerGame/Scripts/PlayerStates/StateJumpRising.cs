using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpRising : BaseState
    {
        public StateJumpRising(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0)
        {
            base.Activate(time);
            Model.UpdateStateName("Jump Rising");
            Model.StandUp();
        }

        public override void OnFixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;

            Model.DirectionCheck();

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
            if (Timer <= 0)
            {
                Model.Grounded(LayerMasks.Platforms);
            }

            // State Walk, while in jump on a steep slope  "Solid"
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Slope))
            {
                Model.Animations.Walk();
                Model.SetState(EPlayerStates.Walk);
            }

            // State Jump Falling
            if (Model.DeltaY < 0)
            {
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling);
            }

            // State JumpRising Attack
            if (Model.HitAttack)
            {
                Model.HitAttack = false;
                Model.SetState(EPlayerStates.JumpRisingAttack, Model.Animations.AirAttack());
            }

        }
    }
}