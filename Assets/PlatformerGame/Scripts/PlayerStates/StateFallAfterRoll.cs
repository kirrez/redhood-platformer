using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateFallAfterRoll : BaseState
    {
        private IAudioManager AudioManager;
        private bool InputBreak;

        public StateFallAfterRoll(IPlayer model)
        {
            Model = model;
            AudioManager = CompositionRoot.GetAudioManager();
        }

        public override void OnEnable(float time = 0)
        {
            base.OnEnable(time);
            Model.UpdateStateName("Fall After Roll");
            InputBreak = false;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            float inputDirection = 0f;
            float currentDirection = 0f;

            base.FixedUpdate();

            if (Model.Horizontal != 0)
            {
                inputDirection = Model.Horizontal;
                currentDirection = Model.GetDirectionX();

                if (inputDirection == currentDirection)
                {
                    //Model.Walk();
                }

                if (inputDirection != currentDirection)
                {
                    Model.DirectionCheck();
                    InputBreak = true;
                }
            }

            if (Model.Horizontal == 0)
            {
                if (InputBreak == false)
                {

                    //Model.Walk();
                }

                if (InputBreak == true)
                {
                    Model.StopHorizontalMotion();
                }
            }

            // when Timer is out we're standing up..
            if (Timer >= 0)
            {
                Timer -= Time.fixedDeltaTime;

                if (Timer < 0)
                {
                    Model.StandUp();
                }
            }

            // Trying to stick to platforms..
            if (Model.Grounded(LayerMasks.PlatformOneWay))
            {
                Model.StickToPlatform();
                Model.PushDown();
                //update
                if (Model.Horizontal == 0)
                {
                    Model.Animations.Idle();
                    Model.SetState(EPlayerStates.Idle);
                }

                if (Model.Horizontal != 0)
                {
                    Model.Animations.Walk();
                    Model.SetState(EPlayerStates.Walk);
                }
                //
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

            // may happen in air after Timer is out
            if (Timer < 0)
            {
                // State JumpFalling
                if (Model.Grounded(LayerMasks.Walkable) == false)
                {
                    Model.Animations.JumpFalling();
                    Model.SetState(EPlayerStates.JumpFalling);
                }
            }
        }
    }
}