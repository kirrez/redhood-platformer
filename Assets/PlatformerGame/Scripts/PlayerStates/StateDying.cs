using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateDying : BaseState
    {

        public StateDying(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0f)
        {
            Timer = time;
            Model.UpdateStateName("Dying");
            Model.Health.HealthChanged = null;
        }

        public override void OnUpdate()
        {
            // nothing
        }

        public override void OnFixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Model.EnableGameOver();
            }
        }

        //protected override void OnHealthChanged()
        //{
        //    // doing just nothing )
        //}
    }
}