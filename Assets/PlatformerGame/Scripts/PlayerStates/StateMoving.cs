using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateMoving : IState
    {
        private Player Model;

        public StateMoving(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            //Model.GetInput();
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

            // Horizontal movement
            //if (Model.PlatformRigidbody != null)
            //{
            //    Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.HorizontalSpeed, 0f) + Model.PlatformRigidbody.velocity;
            //}

            if (Model.PlatformRigidbody == null)
            {
                var y = Model.Facing == EFacing.Left ? -1f : 1f;
                Model.Rigidbody.velocity = new Vector2(y * Time.fixedDeltaTime * Model.HorizontalSpeed, Model.Rigidbody.velocity.y);
            }

            // State Idle
            //if (Model.Horizontal == 0)
            //{
            //    Model.Animations.Idle();
            //    Model.SetState(Model.StateIdle);
            //}

            // State Sit
            //if (Model.Vertical == -1)
            //{
            //    Model.Animations.Sit();
            //    //Model.SetState(Model.StateSit);
            //}

            // State Jump Rising, from ground
            //if (Model.HitJump && Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            //{
            //    //Model.UpdateInAir(true);
            //    Model.HitJump = false;
            //    Model.Animations.JumpRising();
            //    Model.ReleasePlatform();
            //    Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly // Test
            //    Model.SetState(Model.StateJumpRising, 0.75f);
            //    Model.Rigidbody.AddForce(new Vector2(0f, Model.JumpForce));
            //}

            //// State Jump Rising without hitting "Jump" button ))
            //if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            //{
            //    //Model.UpdateInAir(true);
            //    Model.Animations.JumpRising();
            //    Model.SetState(Model.StateJumpRising);
            //}

            //// State Jump Falling
            //if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            //{
            //    //Model.UpdateInAir(true);
            //    Model.Animations.JumpFalling();
            //    Model.SetState(Model.StateJumpFalling);
            //}

            //// Attack Checks. Animations could be different, but they are not ))
            //if (Model.IsAxeAttack())
            //{
            //    Model.ShootAxe();
            //    Model.SetState(Model.StateAttack, Model.Animations.Attack());
            //}

            //if (Model.IsKnifeAttack())
            //{
            //    Model.ShootKnife();
            //    Model.SetState(Model.StateAttack, Model.Animations.Attack());
            //}

            //if (Model.IsHolyWaterAttack())
            //{
            //    Model.ShootHolyWater();
            //    Model.SetState(Model.StateAttack, Model.Animations.Attack());
            //}
        }

        public void Jump()
        {
            Model.ReleasePlatform();
            Model.Animations.JumpRising();
            Model.Rigidbody.AddForce(new Vector2(0f, Model.JumpForce));

            Model.SetState(Model.StateSideJumpRising, 0.75f);
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
            Model.Animations.Idle();
            Model.Rigidbody.velocity = Vector2.zero;

            Model.SetState(Model.StateIdle);
        }
    }
}