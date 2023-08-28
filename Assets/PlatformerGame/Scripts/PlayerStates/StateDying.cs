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
            Model.ChangeHealthUI();
            //Model.SetState(Model.StateDamageTaken);
        }

        public void OnEnable(float time = 0f)
        {
            Model.Timer = time;
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
            throw new System.NotImplementedException();
        }

        public void MoveLeft()
        {
            throw new System.NotImplementedException();
        }

        public void MoveRight()
        {
            throw new System.NotImplementedException();
        }

        public void Fire()
        {
            throw new System.NotImplementedException();
        }

        public void ExtraFire()
        {
            throw new System.NotImplementedException();
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}