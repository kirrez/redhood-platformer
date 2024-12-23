using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateRollDown : BaseState
    {
        private IAudioManager AudioManager;

        public StateRollDown(IPlayer model)
        {
            Model = model;
            AudioManager = CompositionRoot.GetAudioManager();
        }

        public override void OnEnable(float time = 0f)
        {
            base.OnEnable(time);
            Model.UpdateStateName("Roll Down");
            Model.SitDown();

            Model.ResetVelocity();
            Model.RollDown();
            AudioManager.PlayRedhoodSound(EPlayerSounds.RollDown);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Timer -= Time.fixedDeltaTime;

            // State (Jump Falling) FallAfterRoll, something disappeared right beneath your feet or you slided down from a solid surface
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Debug.Log("ROLL -> FALL AFTER ROLL");
                Model.SetState(EPlayerStates.FallAfterRoll, Timer);
            }

            if (Timer <= 0)
            {
                // IDLE

                if (Model.Ceiled(LayerMasks.Solid) == false && Model.Vertical > -1)
                {
                    //Debug.Log("ROLL -> IDLE");
                    Model.Animations.Idle();
                    Model.SetState(EPlayerStates.Idle);
                }

                // SIT

                if (Model.Ceiled(LayerMasks.Solid) == true || Model.Vertical == -1)
                {
                    //Debug.Log("ROLL -> SIT");
                    Model.Animations.Sit();
                    Model.SetState(EPlayerStates.Sit);
                }
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0.1f && Model.Grounded(LayerMasks.Walkable) == false && Model.Ceiled(LayerMasks.Solid) == false)
            {
                Timer = 0f;
                //Model.UpdateInAir(true);
                //Debug.Log("ROLL -> JUMP RISING");
                Model.Animations.JumpRising();
                Model.SetState(EPlayerStates.JumpRising);
            }

           
        }
    }
}