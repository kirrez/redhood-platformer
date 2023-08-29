
namespace Platformer
{
    public interface IPlayerAnimations
    {
        float Idle();
        float Moving();
        float JumpRising();
        float JumpFalling();
        float Dying();
        float Attack();
        float AirAttack();
        float Crouch();
        float CrouchAttack();
        float RollDown();
        float CrouchMoving();
        float DamageTaken();
        float CrouchDamageTaken();
        float Stunned();
    }
}