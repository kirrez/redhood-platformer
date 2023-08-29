using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateAttack : IState
    {
        private Player Model;

        public StateAttack(Player model)
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
            //Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0)
        {
            Model.Timer = time;
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            Model.Timer -= Time.fixedDeltaTime;

            // Carried by platform
            if (Model.Grounded(LayerMasks.PlatformOneWay))
            {
                if (Model.PlatformRigidbody != null)
                {
                    Model.Rigidbody.velocity = Model.PlatformRigidbody.velocity;
                }
            }

            // Steep slope
            if (Model.Grounded(LayerMasks.Slope))
            {
                Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly
            }

            // State Idle, animation fully played
            if (Model.Timer <= 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.Idle();
                Model.SetState(Model.StateIdle);
            }

            // Jump Rising without hitting "Jump", interrupted animation
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpRising();
                Model.SetState(Model.StateJumpRising);
            }

            // jump Falling, interrupted animation
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling);
            }

            // we can shoot any weapon if it's timer = 0
            //if (Model.IsAxeAttack()) Model.ShootAxe();

            //if (Model.IsKnifeAttack()) Model.ShootKnife();

            //if (Model.IsHolyWaterAttack()) Model.ShootHolyWater();
        }

        public void Jump()
        {
        }

        public void MoveLeft()
        {
        }

        public void MoveRight()
        {
        }

        public void Fire()
        {
        }

        public void ExtraFire()
        {
        }

        public void Use()
        {
        }

        public void Stop()
        {
        }

        public void Crouch()
        {
        }

        public void Stand()
        {
        }
    }
}