using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateAttack : BaseState
    {
        public StateAttack(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0)
        {
            base.Activate(time);
            Model.UpdateStateName("Attack");
        }

        public override void OnFixedUpdate()
        {

            Model.AttackCheck();

            Timer -= Time.fixedDeltaTime;

            // Carried by platform
            if (Model.Grounded(LayerMasks.PlatformOneWay))
            {
                Model.StickToPlatform();
            }

            // Steep slope
            if (Model.Grounded(LayerMasks.Slope))
            {
                Model.ResetVelocity();
            }

            // State Idle, animation fully played
            if (Timer <=0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.Idle();
                Model.SetState(EPlayerStates.Idle);
            }

            // Jump Rising without hitting "Jump", interrupted animation
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // jump Falling, interrupted animation
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling);
            }
        }
    }
}