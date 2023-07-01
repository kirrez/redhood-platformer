using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateIdle : BaseState
    {
        public StateIdle(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0f)
        {
            base.Activate(time);
            Model.UpdateStateName("Idle");
            Model.StandUp();
        }

        public override void OnFixedUpdate()
        {
            // Steep slope
            if (Model.Grounded(LayerMasks.Slope))
            {
                Model.ResetVelocity();
            }

            // Carried by MovingPlatform
            if (Model.Grounded(LayerMasks.Platforms))
            {
                Model.StickToPlatform();
            }

            // State Walk
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.Walk();
                Model.SetState(EPlayerStates.Walk);
            }

            // State Sit
            if (Model.Vertical == -1)
            {
                Model.Animations.Sit();
                Model.SetState(EPlayerStates.Sit);
            }

            // State Jump Rising, from ground
            if (Model.HitJump && Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            {
                Model.HitJump = false;
                Model.Animations.JumpRising();
                Model.ReleasePlatform();
                Model.ResetVelocity();//test
                Model.Jump();
                Model.SetState(EPlayerStates.JumpRising, 0.75f);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            {
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet!
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling);
            }

            var attack = Model.GetAttackType();

            // State Attack (old version)
            if (Model.HitAttack)
            {
                Model.HitAttack = false;
                Model.SetState(EPlayerStates.Attack, Model.Animations.Attack());
            }
        }

    }
}