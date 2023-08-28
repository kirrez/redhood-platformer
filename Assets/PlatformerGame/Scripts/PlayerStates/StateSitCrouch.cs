using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateSitCrouch : IState
    {
        private Player Model;

        public StateSitCrouch(Player model)
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

            Model.UpdateFacing();

            // Horizontal movement with checking platform riding
            if (Model.PlatformRigidbody != null)
            {
                Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.CrouchSpeed, 0f) + Model.PlatformRigidbody.velocity;
            }

            if (Model.PlatformRigidbody == null)
            {
                Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.CrouchSpeed, Model.Rigidbody.velocity.y);
            }

            // Sit
            if (Model.Horizontal == 0)
            {
                Model.Animations.Sit();
                Model.SetState(Model.StateSit);
            }

            // Idle and Walk
            if (Model.Vertical > -1 && !Model.Ceiled(LayerMasks.Solid))
            {
                if (Model.Horizontal == 0)
                {
                    //Model.StandUp();
                    Model.Animations.Idle();
                    Model.SetState(Model.StateIdle);
                }
                else if (Model.Horizontal != 0)
                {
                    //Model.StandUp();
                    Model.Animations.Walk();
                    Model.SetState(Model.StateWalk);
                }
            }

            // Roll Down
            if (Model.HitJump && Model.Grounded(LayerMasks.Ground))
            {
                Model.HitJump = false;
                Model.Animations.RollDown();
                Model.SetState(Model.StateRollDown, Model.RollDownTime);
            }

            // State Jump Rising without hitting "Jump" button ))
            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpRising();
                Model.SetState(Model.StateJumpRising);
            }

            // State Jump Falling, something disappeared right beneath your feet!
            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(true);
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling, 0.75f);
            }

            // Attack Checks. Animations could be different, but they are not ))
            if (Model.IsAxeAttack())
            {
                Model.ShootAxe();
                Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            }

            if (Model.IsKnifeAttack())
            {
                Model.ShootKnife();
                Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            }

            if (Model.IsHolyWaterAttack())
            {
                Model.ShootHolyWater();
                Model.SetState(Model.StateSitAttack, Model.Animations.SitAttack());
            }
        }
    }
}