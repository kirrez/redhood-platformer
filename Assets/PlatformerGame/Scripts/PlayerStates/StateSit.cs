namespace Platformer.PlayerStates
{
    public class StateSit : IState
    {
        private Player Model;

        public StateSit(Player model)
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

        public void OnEnable(float time = 0)
        {
            Model.Timer = time;

            Model.SitDown();
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            //TODO: Dubbing?
            //// Carried by MovingPlatform
            //if (Model.Grounded(LayerMasks.Platforms))
            //{
            //    if (Model.PlatformRigidbody != null)
            //    {
            //        Model.Rigidbody.velocity = Model.PlatformRigidbody.velocity;
            //    }
            //}

            if (Model.PlatformRigidbody != null)
            {
                Model.Rigidbody.velocity = Model.PlatformRigidbody.velocity;
            }

            // Crouch
            if (Model.Horizontal != 0)
            {
                Model.Animations.Crouch();
                Model.SetState(Model.StateSitCrouch);
            }

            // Idle
            if (Model.Vertical == 0 && !Model.Ceiled(LayerMasks.Solid))
            {
                Model.Animations.Idle();
                Model.SetState(Model.StateIdle);
            }


            // Jump down from OneWay
            if (Model.HitJump && Model.Grounded(LayerMasks.OneWay))
            {
                Model.HitJump = false;
                Model.JumpDown();
                Model.Animations.JumpFalling();
                //Model.SetState(Model.StateJumpDown, Model.JumpDownTime);
                Model.SetState(Model.StateJumpFalling, 0.75f);
            }

            // Jump down from OneWay
            if (Model.HitJump && Model.Grounded(LayerMasks.PlatformOneWay))
            {
                Model.HitJump = false;
                Model.JumpDown();
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling, 0.75f);
            }

            // Roll Down while on ground !! not Solid (esp. Slope)
            if (Model.HitJump && Model.Grounded(LayerMasks.Ground))
            {
                Model.HitJump = false;
                Model.Animations.RollDown();
                Model.SetState(Model.StateRollDown, Model.RollDownTime);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpRising();
                Model.SetState(Model.StateJumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet!
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling, 0.75f);
            }

            // Attack Checks. Animations could be different, but they are not ))
            if (Model.IsAxeAttack())
            {
                Model.ShootAxe();
                Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            }

            if (Model.IsKnifeAttack())
            {
                Model.ShootKnife();
                Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            }

            if (Model.IsHolyWaterAttack())
            {
                Model.ShootHolyWater();
                Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            }
        }
    }
}