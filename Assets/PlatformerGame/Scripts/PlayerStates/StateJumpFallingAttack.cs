using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateJumpFallingAttack : IState
    {
        private Player Model;

        public StateJumpFallingAttack(Player model)
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
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            Model.UpdateFacing();

            Model.Timer -= Time.fixedDeltaTime;

            // controllable horizontal
            if (Model.Horizontal != 0)
            {
                if (Model.PlatformRigidbody != null)
                {
                    Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.HorizontalSpeed, 0f) + Model.PlatformRigidbody.velocity;
                }

                if (Model.PlatformRigidbody == null)
                {
                    Model.Rigidbody.velocity = new Vector2(Model.Horizontal * Time.fixedDeltaTime * Model.HorizontalSpeed, Model.Rigidbody.velocity.y);
                }
            }

            // State JumpFalling
            if (Model.DeltaY < 0 && Model.Timer <= 0)
            {
                Model.Animations.JumpFalling();
                Model.SetState(Model.StateJumpFalling);
            }

            // State Idle, animation interrupted?? yes..
            if (Model.Horizontal == 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly
                //Model.UpdateInAir(false);
                Model.Animations.Idle();
                Model.SetState(Model.StateIdle);
            }

            // State Walk, animation interrupted
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(false);
                Model.Animations.Walk();
                Model.SetState(Model.StateWalk);
            }

            // we can shoot any weapon if it's timer = 0
            if (Model.IsAxeAttack()) Model.ShootAxe();

            if (Model.IsKnifeAttack()) Model.ShootKnife();

            if (Model.IsHolyWaterAttack()) Model.ShootHolyWater();
        }
    }
}