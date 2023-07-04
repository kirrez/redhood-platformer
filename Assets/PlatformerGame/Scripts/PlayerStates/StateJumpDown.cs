using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpDown : BaseState
    {

        public StateJumpDown(IPlayer model)
        {
            Model = model;
        }

        public override void OnEnable(float time = 0f)
        {
            base.OnEnable(time);

            Model.JumpDown();
        }

        protected override void OnHealthChanged()
        {
            Model.ChangeHealthUI();
            Model.JumpDown();
            Model.SetState(EPlayerStates.DamageTaken);
        }

        public override void FixedUpdate()
        {
            // no base

            Model.UpdateStateName("Jump Down");

            Model.DirectionCheck();

            if (Model.Horizontal != 0)
            {
                Model.Walk();
            }

            // State Jump Falling, Timer finished in air
            if (Timer > 0)
            {
                Timer -= Time.fixedDeltaTime;
                if (Timer <= 0)
                {
                    Model.StandUp();
                    Model.JumpDown();
                    Model.SetState(EPlayerStates.JumpFalling);
                }
            }

            // State Idle -> only when hit "Ground" layer
            if (Model.Grounded(LayerMasks.Ground))
            {
                //Model.UpdateInAir(false);
                Model.StandUp();
                Model.JumpDown();
                Model.Animations.Idle();
                Model.SetState(EPlayerStates.Idle);
            }
        }
    }
}