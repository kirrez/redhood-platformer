using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateIdle : IState
    {
        private Player Model;

        public StateIdle(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            Model.GetInput();
        }

        //public void HealthChanged()
        //{
        //    Model.ChangeHealthUI();
        //    //Model.SetState(Model.StateDamageTaken);
        //}

        //public void OnEnable(float time = 0f)
        //{
        //    Model.Timer = time;

        //    Model.StandUp();
        //}

        public void FixedUpdate()
        {
            //Model.SetDeltaY();
            //Model.UpdateAttackTimers();

            //// Steep slope
            //if (Model.Grounded(LayerMasks.Slope))
            //{
            //    Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly
            //}

            //// Carried by MovingPlatform
            //if (Model.Grounded(LayerMasks.Platforms))
            //{
            //    if (Model.PlatformRigidbody != null)
            //    {
            //        Model.Rigidbody.velocity = Model.PlatformRigidbody.velocity;
            //    }
            //}

            //// State Walk
            //if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            //{
            //    Model.Animations.Walk();
            //    Model.SetState(Model.StateWalk);
            //}

            //// State Sit
            //if (Model.Vertical == -1)
            //{
            //    Model.Animations.Sit();
            //    //Model.SetState(Model.StateSit);
            //}

            //// State Jump Rising, from ground
            //if (Model.HitJump && Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            //{
            //    Model.HitJump = false;
            //    Model.Animations.JumpRising();
            //    Model.ReleasePlatform();
            //    Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly // test
            //    Model.Rigidbody.AddForce(new Vector2(0f, Model.JumpForce));
            //    Model.SetState(Model.StateJumpRising, 0.75f);
            //}

            //// State Jump Rising without hitting "Jump" button ))
            //if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            //{
            //    Model.Animations.JumpRising();
            //    Model.SetState(Model.StateJumpRising);
            //}

            //// State Jump Falling, something disappeared right beneath your feet!
            //if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            //{
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
            Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly // Test
            Model.Rigidbody.AddForce(new Vector2(0f, Model.JumpForce));

            Model.SetState(Model.StateJumpRising, 0.75f);
        }

        public void MoveLeft()
        {
            Model.Animations.Moving();

            Model.Rigidbody.velocity = new Vector2(-Time.fixedDeltaTime * Model.HorizontalSpeed, Model.Rigidbody.velocity.y);

            Model.Renderer.flipX = true;
            Model.Facing = EFacing.Left;

            var newPosition = new Vector2(-Model.StandingFirePointX, Model.StandingFirePoint.localPosition.y);
            Model.StandingFirePoint.localPosition = newPosition;

            newPosition = new Vector2(-Model.SittingFirePointX, Model.SittingFirePoint.localPosition.y);
            Model.SittingFirePoint.localPosition = newPosition;

            Model.SetState(Model.StateMoving);
        }

        public void MoveRight()
        {
            Model.Animations.Moving();

            Model.Rigidbody.velocity = new Vector2(Time.fixedDeltaTime * Model.HorizontalSpeed, Model.Rigidbody.velocity.y);

            Model.Renderer.flipX = false;
            Model.Facing = EFacing.Right;

            var newPosition = new Vector2(Model.StandingFirePointX, Model.StandingFirePoint.localPosition.y);
            Model.StandingFirePoint.localPosition = newPosition;

            newPosition = new Vector2(Model.SittingFirePointX, Model.SittingFirePoint.localPosition.y);
            Model.SittingFirePoint.localPosition = newPosition;

            Model.SetState(Model.StateMoving);
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
            Model.Animations.Crouch();
            Model.SetState(Model.StateCrouch);
        }

        public void Stand()
        {
        }
    }
}