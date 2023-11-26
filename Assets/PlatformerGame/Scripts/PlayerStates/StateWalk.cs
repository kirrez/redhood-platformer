
namespace Platformer.PlayerStates
{
    public class StateWalk : BaseState
    {
        public StateWalk(IPlayer model)
        {
            Model = model;
        }

        public override void OnEnable(float time = 0)
        {
            base.OnEnable(time);
            Model.UpdateStateName("Walk");
            Model.StandUp();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Model.DirectionCheck();

            // Horizontal movement
            Model.Walk();

            // State Idle
            if (Model.Horizontal == 0)
            {
                Model.Animations.Idle();
                Model.SetState(EPlayerStates.Idle);
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
                //Model.UpdateInAir(true);
                Model.HitJump = false;
                Model.Animations.JumpRising();
                Model.ReleasePlatform();
                Model.ResetVelocity(); // test
                Model.Jump();
                //Model.PlayRedhoodSound(EPlayerSounds.Jump);

                Model.SetState(EPlayerStates.JumpRising, 0.75f);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // State Jump Falling
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
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