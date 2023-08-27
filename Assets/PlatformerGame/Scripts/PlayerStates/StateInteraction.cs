namespace Platformer.PlayerStates
{
    public class StateInteraction : IState
    {
        private Player Model;

        public StateInteraction(Player model)
        {
            Model = model;
        }

        public void Update()
        {
            // no player control input
            Model.GetInteractionInput();
        }

        public void HealthChanged()
        {
            Model.ChangeHealthUI();
            Model.SetState(EPlayerStates.DamageTaken);
        }

        public void OnEnable(float time = 0f)
        {
            Model.Timer = time;
            Model.Animations.Idle();
            Model.ResetVelocity();
            Model.InactivateCollider(true);
        }

        public void FixedUpdate()
        {
            // doing nothing, no base
        }
    }
}