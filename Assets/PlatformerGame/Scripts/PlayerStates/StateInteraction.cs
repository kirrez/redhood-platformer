using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateInteraction : BaseState
    {
        public StateInteraction(IPlayer model)
        {
            Model = model;
        }

        public override void OnEnable(float time = 0f)
        {
            Timer = time;
            Model.UpdateStateName("Interaction");
            Model.Animations.Idle();
            Model.ResetVelocity();
            Model.InactivateCollider(true);
            Model.Health.HealthChanged = null; //cleaning previous handler
        }
        public override void Update()
        {
            // no player control input
            Model.GetInteractionInput();
        }

        public override void FixedUpdate()
        {
            // doing nothing, no base
        }
    }
}