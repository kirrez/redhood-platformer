
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpFalling : BaseState
    {
        private IAudioManager AudioManager;

        public StateJumpFalling(IPlayer model)
        {
            Model = model;
            AudioManager = CompositionRoot.GetAudioManager();
        }

        public override void OnEnable(float time = 0)
        {
            base.OnEnable(time);
            Model.UpdateStateName("Jump Falling");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Model.DirectionCheck();

            //StandUp happens immediately or after a delay, if something is passed in Activate()
            if (Timer >= 0)
            {
                Timer -= Time.fixedDeltaTime;

                if (Timer < 0)
                {
                    Model.StandUp();
                }
            }

            // Horizontal movement, should change to some more smooth logic for controllable falling
            if (Model.Horizontal != 0)
            {
                Model.Walk();
            }

            // Trying to stick..
            if (Model.Grounded(LayerMasks.Platforms))
            {
                Model.StickToPlatform();
            }


            // State Idle
            if (Model.Horizontal == 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.ResetVelocity();
                //Model.UpdateInAir(false);
                Model.Animations.Idle();
                AudioManager.PlayRedhoodSound(EPlayerSounds.Landing);

                Model.SetState(EPlayerStates.Idle);
            }

            // State Walk
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(false);
                Model.Animations.Walk();
                AudioManager.PlayRedhoodSound(EPlayerSounds.Landing);

                Model.SetState(EPlayerStates.Walk);
            }

            // State JumpRising, happens after mushroom jump

            if (Model.DeltaY > 0)
            {
                Model.Animations.JumpRising();

                Model.SetState(EPlayerStates.JumpRising);
            }

            // Attack Checks. Animations could be different, but they are not ))
            if (Model.IsAxeAttack())
            {
                Model.ShootAxe();
                Model.SetState(EPlayerStates.JumpFallingAttack, Model.Animations.AirAttack());
            }

            if (Model.IsKnifeAttack())
            {
                Model.ShootKnife();
                Model.SetState(EPlayerStates.JumpFallingAttack, Model.Animations.AirAttack());
            }

            if (Model.IsHolyWaterAttack())
            {
                Model.ShootHolyWater();
                Model.SetState(EPlayerStates.JumpFallingAttack, Model.Animations.AirAttack());
            }
        }
    }
}