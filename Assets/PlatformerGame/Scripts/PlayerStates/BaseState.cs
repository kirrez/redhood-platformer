namespace Platformer.PlayerStates
{
    public interface IState
    {
        void Update();
        void FixedUpdate();
        void OnEnable(float time);
    }
}