
namespace Platformer.PlayerStates
{
    public class StateSitCrouch : BaseState
    {
        public StateSitCrouch(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0)
        {
            base.Activate(time);
            Model.UpdateStateName("Crouch");
            Model.SitDown();
        }

        public override void OnFixedUpdate()
        {
            Model.DirectionCheck();

            // Horizontal movement with checking platform riding
            Model.Crouch();

            // Sit
            if (Model.Horizontal == 0)
            {
                Model.Animations.Sit();
                Model.SetState(EPlayerStates.Sit);
            }

            // Idle and Walk
            if (Model.Vertical > -1 && !Model.Ceiled(LayerMasks.Solid))
            {
                if (Model.Horizontal == 0)
                {
                    //Model.StandUp();
                    Model.Animations.Idle();
                    Model.SetState(EPlayerStates.Idle);
                }
                else if (Model.Horizontal != 0)
                {
                    //Model.StandUp();
                    Model.Animations.Walk();
                    Model.SetState(EPlayerStates.Walk);
                }
            }

            // State SitAttack
            if (Model.HitAttack)
            {
                Model.HitAttack = false;
                Model.SetState(EPlayerStates.SitAttack, Model.Animations.SitAttack());
            }

            // Roll Down
            if (Model.HitJump && Model.Grounded(LayerMasks.Ground))
            {
                Model.HitJump = false;
                Model.Animations.RollDown();
                Model.SetState(EPlayerStates.RollDown, Model.RollDownTime);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet!
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling, 0.75f);
            }
        }
    }
}