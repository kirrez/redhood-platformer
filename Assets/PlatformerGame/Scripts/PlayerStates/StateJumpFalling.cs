using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpFalling : IState
    {
        private Player Model;

        public StateJumpFalling(Player model)
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
        }

        public void FixedUpdate()
        {
            //Model.SetDeltaY();
            //Model.UpdateAttackTimers();

            //Model.UpdateFacing();

            //StandUp happens immediately or after a delay, if something is passed in Activate()
            //if (Model.Timer >= 0)
            //{
            //    Model.Timer -= Time.fixedDeltaTime;

            //    if (Model.Timer < 0)
            //    {
            //        Model.StandUp();
            //    }
            //}

            // Horizontal movement, should change to some more smooth logic for controllable falling
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

            // Trying to stick..
            //if (Model.Grounded(LayerMasks.Platforms) && Model.PlatformRigidbody != null)
            //{
            //    Model.Rigidbody.velocity = Model.PlatformRigidbody.velocity;
            //}

            // State Idle
            //if (Model.Horizontal == 0 && Model.Grounded(LayerMasks.Walkable))
            //{
            //    Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly
            //    //Model.UpdateInAir(false);
            //    Model.Animations.Idle();
            //    Model.SetState(Model.StateIdle);
            //}

            if (Model.Grounded(LayerMasks.Walkable))
            {
                Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly
                Model.Animations.Idle();

                Model.SetState(Model.StateIdle);
                return;
            }

            // State Walk
            //if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            //{
            //    //Model.UpdateInAir(false);
            //    Model.Animations.Walk();
            //    Model.SetState(Model.StateWalk);
            //}

            // Attack Checks. Animations could be different, but they are not ))
            //if (Model.IsAxeAttack())
            //{
            //    Model.ShootAxe();
            //    Model.SetState(Model.StateJumpFallingAttack, Model.Animations.AirAttack());
            //}

            //if (Model.IsKnifeAttack())
            //{
            //    Model.ShootKnife();
            //    Model.SetState(Model.StateJumpFallingAttack, Model.Animations.AirAttack());
            //}

            //if (Model.IsHolyWaterAttack())
            //{
            //    Model.ShootHolyWater();
            //    Model.SetState(Model.StateJumpFallingAttack, Model.Animations.AirAttack());
            //}
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

            Model.SetState(Model.StateSideJumpFalling);
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

            Model.SetState(Model.StateSideJumpFalling);
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
        }

        public void Stand()
        {
        }
    }
}