using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateSit : BaseState
    {
        public StateSit(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0)
        {
            base.Activate(time);
            Model.UpdateStateName("Sit");
            Model.SitDown();
        }

        public override void OnFixedUpdate()
        {

            // Push Down, should i use it while sitting?
            //if (Model.Vertical < 0)
            //{
            //    Model.PushDown();
            //}

            // Carried by platform
            //if (Model.Grounded(LayerMasks.Platforms))
            //{
            //    Model.StickToPlatform();
            //}
            //Model.StickToPlatform();

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

            // State SitAttack
            if (Model.HitAttack)
            {
                Model.HitAttack = false;
                Model.SetState(EPlayerStates.SitAttack, Model.Animations.SitAttack());
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
        }
    }
}