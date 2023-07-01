using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateStunned : BaseState
    {
        public StateStunned(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0)
        {
            Timer = time;
            Model.UpdateStateName("Stunned");

            Model.Animations.Stunned();
            Model.ResetVelocity();
            Model.InactivateCollider(true);
            Model.Health.HealthChanged = null;
        }

        public override void OnUpdate()
        {
            // only input without attack and movement
            // Model.GetInteractionInput();
        }

        public override void OnFixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                //Idle
                if (Model.Grounded(LayerMasks.Walkable))
                {
                    Model.InactivateCollider(false);
                    Model.Animations.Idle();
                    Model.SetState(EPlayerStates.Idle);
                }

                //JumpFalling
                if (!Model.Grounded(LayerMasks.Walkable))
                {
                    Model.InactivateCollider(false);
                    Model.Animations.JumpFalling();
                    Model.SetState(EPlayerStates.JumpFalling);
                }
            }
        }
    }
}