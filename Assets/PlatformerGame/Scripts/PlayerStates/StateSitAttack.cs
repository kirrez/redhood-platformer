using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateSitAttack : IState
    {
        private Player Model;

        public StateSitAttack(Player model)
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
            Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0)
        {
            Model.Timer = time;

            Model.SitDown();
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            Model.Timer -= Time.fixedDeltaTime;

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
            if (Model.Timer <= 0 && Model.Vertical > -1 && !Model.Ceiled(LayerMasks.Ground + LayerMasks.Slope))
            {
                Model.Animations.Idle();
                Model.SetState(Model.StateIdle);
            }

            // State Sit, animation fully played
            if (Model.Timer <= 0)
            {
                Model.Animations.Sit();
                Model.SetState(Model.StateSit);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpRising();
                Model.SetState(Model.StateJumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet!
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling);
            }

            // we can shoot weapon if it's timer = 0 (but not axe and HW)
            if (Model.IsKnifeAttack()) Model.ShootKnife();
        }
    }
}