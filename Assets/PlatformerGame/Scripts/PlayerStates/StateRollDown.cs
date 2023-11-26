using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateRollDown : BaseState
    {
        public StateRollDown(IPlayer model)
        {
            Model = model;
        }

        public override void OnEnable(float time = 0f)
        {
            base.OnEnable(time);
            Model.UpdateStateName("Roll Down");
            Model.SitDown();

            Model.ResetVelocity();
            Model.RollDown();
            Model.PlayRedhoodSound(EPlayerSounds.RollDown);
        }

        public override void Update()
        {
            //if (Timer <= 0)
            //{
            //    Model.GetInput();
            //}
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                // Idle and Walk
                if (!Model.Ceiled(LayerMasks.Solid) && Model.Vertical > -1)
                {
                    if (Model.Horizontal == 0)
                    {
                        Model.Animations.Idle();
                        Model.SetState(EPlayerStates.Idle);
                    }
                    else if (Model.Horizontal != 0)
                    {
                        Model.Animations.Walk();
                        Model.SetState(EPlayerStates.Walk);
                    }
                }

                // Sit and Crouch
                if (Model.Ceiled(LayerMasks.Solid) || Model.Vertical == -1)
                {
                    if (Model.Horizontal == 0)
                    {
                        Model.Animations.Sit();
                        Model.SetState(EPlayerStates.Sit);
                    }
                    else if (Model.Horizontal != 0)
                    {
                        Model.Animations.Crouch();
                        Model.SetState(EPlayerStates.SitCrouch);
                    }
                }
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Timer = 0f;
                //Model.UpdateInAir(true);
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet or you slided down from a solid surface
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Timer = 0f;
                //Model.UpdateInAir(true);
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling, 0.75f);
            }
        }
    }
}