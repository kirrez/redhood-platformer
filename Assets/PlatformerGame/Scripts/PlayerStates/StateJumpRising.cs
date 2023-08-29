using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpRising : IState
    {
        private Player Model;

        public StateJumpRising(Player model)
        {
            Model = model;
        }

        public void HealthChanged()
        {
            //Model.ChangeHealthUI();
            //Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0)
        {
            //Model.Timer = time;

            //Model.StandUp();
        }

        public void FixedUpdate()
        {
            //Model.SetDeltaY();
            //Model.UpdateAttackTimers();

            //Model.UpdateFacing();

            //Model.Timer -= Time.fixedDeltaTime;

            // Push Down
            //if (Model.Vertical < 0)
            //{
            //    Model.Rigidbody.velocity = new Vector2(Model.Rigidbody.velocity.x, Model.Rigidbody.velocity.y + Model.PushDownForce * Time.fixedDeltaTime * (-1));
            //}

            // Horizontal movement, controllable jump
            //if (Model.Horizontal != 0)
            //{
            //    if (Model.PlatformRigidbody != null)
            //    {
            //        Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.HorizontalSpeed, 0f) + Model.PlatformRigidbody.velocity;
            //    }

            //    if (Model.PlatformRigidbody == null)
            //    {
            //        Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.HorizontalSpeed, Model.Rigidbody.velocity.y);
            //    }
            //}

            // Check MovingPlatform again after cooldown
            //if (Model.Timer <= 0)
            //{
            //    Model.Grounded(LayerMasks.Platforms);
            //}

            // State Walk, while in jump on a steep slope  "Solid"
            //if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Slope))
            //{
            //    Model.Animations.Walk();
            //    Model.SetState(Model.StateWalk);
            //}

            // State Jump Falling
            //if (Model.DeltaY < 0)
            //{
            //    Model.Animations.JumpFalling();
            //    Model.SetState(Model.StateJumpFalling);
            //}

            if (Model.Rigidbody.velocity.y < 0f)
            {
                Model.Animations.JumpFalling();

                Model.SetState(Model.StateJumpFalling);
                return;
            }

            // Attack Checks. Animations could be different, but they are not ))
            //if (Model.IsAxeAttack())
            //{
            //    Model.ShootAxe();
            //    Model.SetState(Model.StateJumpRisingAttack, Model.Animations.AirAttack());
            //}

            //if (Model.IsKnifeAttack())
            //{
            //    Model.ShootKnife();
            //    Model.SetState(Model.StateJumpRisingAttack, Model.Animations.AirAttack());
            //}

            //if (Model.IsHolyWaterAttack())
            //{
            //    Model.ShootHolyWater();
            //    Model.SetState(Model.StateJumpRisingAttack, Model.Animations.AirAttack());
            //}
        }

        public void Update()
        {
            //Model.GetInput();
        }

        public void Jump()
        {
        }

        public void MoveLeft()
        {
            Model.Rigidbody.velocity = new Vector2(-Time.fixedDeltaTime * Model.HorizontalSpeed, Model.Rigidbody.velocity.y);

            Model.Renderer.flipX = true;
            Model.Facing = EFacing.Left;

            var newPosition = new Vector2(-Model.StandingFirePointX, Model.StandingFirePoint.localPosition.y);
            Model.StandingFirePoint.localPosition = newPosition;

            newPosition = new Vector2(-Model.SittingFirePointX, Model.SittingFirePoint.localPosition.y);
            Model.SittingFirePoint.localPosition = newPosition;

            Model.SetState(Model.StateSideJumpRising);
        }

        public void MoveRight()
        {
            Model.Rigidbody.velocity = new Vector2(Time.fixedDeltaTime * Model.HorizontalSpeed, Model.Rigidbody.velocity.y);

            Model.Renderer.flipX = false;
            Model.Facing = EFacing.Right;

            var newPosition = new Vector2(Model.StandingFirePointX, Model.StandingFirePoint.localPosition.y);
            Model.StandingFirePoint.localPosition = newPosition;

            newPosition = new Vector2(Model.SittingFirePointX, Model.SittingFirePoint.localPosition.y);
            Model.SittingFirePoint.localPosition = newPosition;

            Model.SetState(Model.StateSideJumpRising);
        }

        public void Fire()
        {
            throw new System.NotImplementedException();
        }

        public void ExtraFire()
        {
            throw new System.NotImplementedException();
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
        }

        public void Crouch()
        {
            //TODO: it works?
            Model.Rigidbody.velocity = new Vector2(Model.Rigidbody.velocity.x, Model.Rigidbody.velocity.y + Model.PushDownForce * Time.fixedDeltaTime * (-1));
        }

        public void Stand()
        {
            throw new System.NotImplementedException();
        }
    }
}