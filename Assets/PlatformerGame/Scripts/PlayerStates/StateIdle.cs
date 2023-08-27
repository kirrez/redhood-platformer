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

        public void HealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(EPlayerStates.DamageTaken);
        }

        public void OnEnable(float time = 0f)
        {
            Model.Timer = time;

            Model.StandUp();
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            // Steep slope
            if (Model.Grounded(LayerMasks.Slope))
            {
                Model.ResetVelocity();
            }

            // Carried by MovingPlatform
            if (Model.Grounded(LayerMasks.Platforms))
            {
                Model.StickToPlatform();
            }

            // State Walk
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.Walk();
                Model.SetState(EPlayerStates.Walk);
            }

            // State Sit
            if (Model.Vertical == -1)
            {
                Model.Animations.Sit();
                Model.SetState(EPlayerStates.Sit);
            }

            // State Jump Rising, from ground
            if (Model.HitJump && Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            {
                Model.HitJump = false;
                Model.Animations.JumpRising();
                Model.ReleasePlatform();
                Model.ResetVelocity();//test
                Model.Jump();
                Model.SetState(EPlayerStates.JumpRising, 0.75f);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            {
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet!
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling);
            }

            // Attack Checks. Animations could be different, but they are not ))
            if (Model.IsAxeAttack())
            {
                Model.ShootAxe();
                Model.SetState(EPlayerStates.Attack, Model.Animations.Attack());
            }

            if (Model.IsKnifeAttack())
            {
                Model.ShootKnife();
                Model.SetState(EPlayerStates.Attack, Model.Animations.Attack());
            }

            if (Model.IsHolyWaterAttack())
            {
                Model.ShootHolyWater();
                Model.SetState(EPlayerStates.Attack, Model.Animations.Attack());
            }
        }

    }
}