namespace Platformer.PlayerStates
{
    public interface IState
    {
        void Update();
        void FixedUpdate();
        //void OnEnable(float time);

        //Refactoring
        void Jump();
        void MoveLeft();
        void MoveRight();
        void Fire();
        void ExtraFire();
        void Use();
        void Stop();
    }
}