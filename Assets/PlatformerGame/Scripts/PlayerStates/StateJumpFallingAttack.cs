using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpFallingAttack : BaseState
    {
        public StateJumpFallingAttack(IPlayer model)
        {
            Model = model;
        }

        public override void OnEnable(float time = 0)
        {
            base.OnEnable(time);
            Model.UpdateStateName("Jump Falling Attack");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Model.DirectionCheck();

            Timer -= Time.fixedDeltaTime;

            // controllable horizontal
            if (Model.Horizontal != 0)
            {
                Model.Walk();
            }

            // State JumpFalling
            if (Model.DeltaY < 0 && Timer <= 0)
            {
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling);
            }

            // State Idle, animation interrupted?? yes..
            if (Model.Horizontal == 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.ResetVelocity();
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

            // we can shoot any weapon if it's timer = 0
            if (Model.IsAxeAttack()) Model.ShootAxe();

            if (Model.IsKnifeAttack()) Model.ShootKnife();

            if (Model.IsHolyWaterAttack()) Model.ShootHolyWater();
        }
    }
}