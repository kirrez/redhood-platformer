using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateStunned : IState
    {
        private Player Model;

        public StateStunned(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            // only input without attack and movement
            // Model.GetInteractionInput();
        }

        public void HealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0)
        {
            Model.Timer = time;

            Model.Animations.Stunned();
            Model.ResetVelocity();
            Model.InactivateCollider(true);
        }

        public void FixedUpdate()
        {
            // no base

            Model.Timer -= Time.fixedDeltaTime;

            if (Model.Timer <= 0)
            {
                //Idle
                if (Model.Grounded(LayerMasks.Walkable))
                {
                    Model.InactivateCollider(false);
                    Model.Animations.Idle();
                    Model.SetState(Model.StateIdle);
                }

                //JumpFalling
                if (!Model.Grounded(LayerMasks.Walkable))
                {
                    Model.InactivateCollider(false);
                    Model.Animations.JumpFalling();
                    Model.SetState(Model.StateJumpFalling);
                }
            }
        }
    }
}