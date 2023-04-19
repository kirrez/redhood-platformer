using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateSitAttack : BaseState
    {
        public StateSitAttack(IPlayer model)
        {
            Model = model;
        }

        public override void Activate(float time = 0)
        {
            base.Activate(time);
            Model.UpdateStateName("Sit Attack");
            Model.SitDown();
        }

        public override void OnFixedUpdate()
        {

            Model.AttackCheck();

            Timer -= Time.fixedDeltaTime;

            // Push Down
            if (Model.Vertical < 0)
            {
                Model.PushDown();
            }

            // Carried by platform
            if (Model.Grounded(LayerMasks.PlatformOneWay))
            {
                Model.StickToPlatform();
            }

            // State Idle (instead of Attack), animation fully played
            if (Timer <= 0 && Model.Vertical > -1 && !Model.Ceiled(LayerMasks.Ground + LayerMasks.Slope))
            {
                Model.Animations.Idle();
                Model.SetState(EPlayerStates.Idle);
            }

            // State Sit, animation fully played
            if (Timer <= 0)
            {
                Model.Animations.Sit();
                Model.SetState(EPlayerStates.Sit);
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
                Model.SetState(EPlayerStates.JumpFalling);
            }
        }
    }
}