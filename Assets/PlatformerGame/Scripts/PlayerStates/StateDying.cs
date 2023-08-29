using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateDying : IState
    {
        private Player Model;

        public StateDying(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            // no player control input
            //Model.GetInput();
        }

        public void HealthChanged()
        {
            //Model.ChangeHealthUI();
            //Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0f)
        {
            //Model.Timer = time;
        }

        public void FixedUpdate()
        {
            // no base

            Model.Timer -= Time.fixedDeltaTime;

            if (Model.Timer <= 0)
            {
                Model.EnableGameOver();
            }
        }

        public void Jump()
        {
        }

        public void MoveLeft()
        {
        }

        public void MoveRight()
        {
        }

        public void Fire()
        {
        }

        public void ExtraFire()
        {
        }

        public void Use()
        {
        }

        public void Stop()
        {
        }

        public void Crouch()
        {
        }

        public void Stand()
        {
        }
    }
}