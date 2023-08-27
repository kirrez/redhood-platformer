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
            Model.SetState(EPlayerStates.DamageTaken);
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
    }
}