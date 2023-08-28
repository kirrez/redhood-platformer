using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpDown : IState
    {
        private Player Model;

        public StateJumpDown(Player model)
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

        public void OnEnable(float time = 0f)
        {
            Model.Timer = time;

            Model.JumpDown();
        }

        public void OnHealthChanged()
        {
            Model.ChangeHealthUI();
            Model.JumpDown();
            Model.SetState(Model.StateDamageTaken);
        }

        public void FixedUpdate()
        {
            // no base

            Model.UpdateFacing();

            if (Model.Horizontal != 0)
            {
                Model.Walk();
            }

            // State Jump Falling, Timer finished in air
            if (Model.Timer > 0)
            {
                Model.Timer -= Time.fixedDeltaTime;
                if (Model.Timer <= 0)
                {
                    Model.StandUp();
                    Model.JumpDown();
                    Model.SetState(Model.StateJumpFalling);
                }
            }

            // State Idle -> only when hit "Ground" layer
            if (Model.Grounded(LayerMasks.Ground))
            {
                //Model.UpdateInAir(false);
                Model.StandUp();
                Model.JumpDown();
                Model.Animations.Idle();
                Model.SetState(Model.StateIdle);
            }
        }
    }
}