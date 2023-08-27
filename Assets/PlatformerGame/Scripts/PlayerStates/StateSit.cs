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
            Model.SetState(EPlayerStates.DamageTaken);
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

            // Carried by MovingPlatform
            if (Model.Grounded(LayerMasks.Platforms))
            {
                Model.StickToPlatform();
            }
            Model.StickToPlatform();

            // Crouch
            if (Model.Horizontal != 0)
            {
                Model.Animations.Crouch();
                Model.SetState(EPlayerStates.SitCrouch);
            }

            // Idle
            if (Model.Vertical == 0 && !Model.Ceiled(LayerMasks.Solid))
            {
                Model.Animations.Idle();
                Model.SetState(EPlayerStates.Idle);
            }


            // Jump down from OneWay
            if (Model.HitJump && Model.Grounded(LayerMasks.OneWay))
            {
                Model.HitJump = false;
                Model.JumpDown();
                Model.Animations.JumpFalling();
                //Model.SetState(EPlayerStates.JumpDown, Model.JumpDownTime);
                Model.SetState(EPlayerStates.JumpFalling, 0.75f);
            }

            // Jump down from OneWay
            if (Model.HitJump && Model.Grounded(LayerMasks.PlatformOneWay))
            {
                Model.HitJump = false;
                Model.JumpDown();
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling, 0.75f);
            }

            // Roll Down while on ground !! not Solid (esp. Slope)
            if (Model.HitJump && Model.Grounded(LayerMasks.Ground))
            {
                Model.HitJump = false;
                Model.Animations.RollDown();
                Model.SetState(EPlayerStates.RollDown, Model.RollDownTime);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet!
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling, 0.75f);
            }

            // Attack Checks. Animations could be different, but they are not ))
            if (Model.IsAxeAttack())
            {
                Model.ShootAxe();
                Model.SetState(EPlayerStates.SitAttack, Model.Animations.SitAttack());
            }

            if (Model.IsKnifeAttack())
            {
                Model.ShootKnife();
                Model.SetState(EPlayerStates.SitAttack, Model.Animations.SitAttack());
            }

            if (Model.IsHolyWaterAttack())
            {
                Model.ShootHolyWater();
                Model.SetState(EPlayerStates.SitAttack, Model.Animations.SitAttack());
            }
        }
    }
}