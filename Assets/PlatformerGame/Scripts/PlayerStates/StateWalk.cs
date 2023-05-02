
namespace Platformer.PlayerStates
{
    public class StateWalk : BaseState
    {
        public StateWalk(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0)
        {
            base.Activate(time);
            Model.UpdateStateName("Walk");
            Model.StandUp();
        }

        public override void OnFixedUpdate()
        {
            Model.DirectionCheck();

            // Horizontal movement with checking platform riding
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

            // State Attack
            if (Model.HitAttack)
            {
                Model.HitAttack = false;
                Model.SetState(EPlayerStates.Attack, Model.Animations.Attack());
            }

            // State Jump Rising, from ground
            if (Model.HitJump && Model.Grounded(LayerMasks.Walkable) && !Model.StandingCeiled(LayerMasks.Solid))
            {
                //Model.UpdateInAir(true);
                Model.HitJump = false;
                Model.ResetVelocity();

                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
                Model.Jump();
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
        }

    }
}