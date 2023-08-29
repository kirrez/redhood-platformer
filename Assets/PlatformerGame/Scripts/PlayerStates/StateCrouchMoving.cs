using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateCrouchMoving : IState
    {
        private Player Model;

        public StateCrouchMoving(Player model)
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

            //Model.SitDown();
        }

        public void FixedUpdate()
        {
            //Model.SetDeltaY();
            //Model.UpdateAttackTimers();

            //Model.UpdateFacing();

            //// Horizontal movement with checking platform riding
            //if (Model.PlatformRigidbody != null)
            //{
            //    Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.CrouchSpeed, 0f) + Model.PlatformRigidbody.velocity;
            //}

            if (Model.PlatformRigidbody == null)
            {
                var y = Model.Facing == EFacing.Left ? -1f : 1f;
                Model.Rigidbody.velocity = new Vector2(y * Time.fixedDeltaTime * Model.CrouchSpeed, Model.Rigidbody.velocity.y);
            }

            //// Sit
            //if (Model.Horizontal == 0)
            //{
            //    Model.Animations.Sit();
            //    Model.SetState(Model.StateSit);
            //}

            //// Idle and Walk
            //if (Model.Vertical > -1 && !Model.Ceiled(LayerMasks.Solid))
            //{
            //    if (Model.Horizontal == 0)
            //    {
            //        //Model.StandUp();
            //        Model.Animations.Idle();
            //        Model.SetState(Model.StateIdle);
            //    }
            //    else if (Model.Horizontal != 0)
            //    {
            //        //Model.StandUp();
            //        Model.Animations.Walk();
            //        Model.SetState(Model.StateWalk);
            //    }
            //}

            //// Roll Down
            //if (Model.HitJump && Model.Grounded(LayerMasks.Ground))
            //{
            //    Model.HitJump = false;
            //    Model.Animations.RollDown();
            //    Model.SetState(Model.StateRollDown, Model.RollDownTime);
            //}

            //// State Jump Rising without hitting "Jump" button ))
            //if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            //{
            //    //Model.UpdateInAir(true);
            //    Model.Animations.JumpRising();
            //    Model.SetState(Model.StateJumpRising);
            //}

            //// State Jump Falling, something disappeared right beneath your feet!
            //if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            //{
            //    //Model.UpdateInAir(true);
            //    Model.Animations.JumpFalling();
            //    Model.SetState(Model.StateJumpFalling, 0.75f);
            //}

            //// Attack Checks. Animations could be different, but they are not ))
            //if (Model.IsAxeAttack())
            //{
            //    Model.ShootAxe();
            //    Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            //}

            //if (Model.IsKnifeAttack())
            //{
            //    Model.ShootKnife();
            //    Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            //}

            //if (Model.IsHolyWaterAttack())
            //{
            //    Model.ShootHolyWater();
            //    Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
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
            Model.Animations.Crouch();
            Model.SetState(Model.StateCrouch);
        }

        public void Crouch()
        {
        }

        public void Stand()
        {
            Model.Animations.Moving();
            Model.SetState(Model.StateMoving);
        }
    }
}