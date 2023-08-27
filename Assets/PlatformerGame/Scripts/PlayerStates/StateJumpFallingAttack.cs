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
            Model.SetState(EPlayerStates.DamageTaken);
        }

        public void OnEnable(float time = 0)
        {
            Model.Timer = time;
        }

        public void FixedUpdate()
        {
            Model.SetDeltaY();
            Model.UpdateAttackTimers();

            Model.DirectionCheck();

            Model.Timer -= Time.fixedDeltaTime;

            // controllable horizontal
            if (Model.Horizontal != 0)
            {
                Model.Walk();
            }

            // State JumpFalling
            if (Model.DeltaY < 0 && Model.Timer <= 0)
            {
                Model.Animations.JumpFalling();
                Model.SetState(EPlayerStates.JumpFalling);
            }

            // State Idle, animation interrupted?? yes..
            if (Model.Horizontal == 0 && Model.Grounded(LayerMasks.Walkable))
            {
                Model.ResetVelocity();
                //Model.UpdateInAir(false);
                Model.Animations.Idle();
                Model.SetState(EPlayerStates.Idle);
            }

            // State Walk, animation interrupted
            if (Model.Horizontal != 0 && Model.Grounded(LayerMasks.Walkable))
            {
                //Model.UpdateInAir(false);
                Model.Animations.Walk();
                Model.SetState(EPlayerStates.Walk);
            }

            // we can shoot any weapon if it's timer = 0
            if (Model.IsAxeAttack()) Model.ShootAxe();

            if (Model.IsKnifeAttack()) Model.ShootKnife();

            if (Model.IsHolyWaterAttack()) Model.ShootHolyWater();
        }
    }
}