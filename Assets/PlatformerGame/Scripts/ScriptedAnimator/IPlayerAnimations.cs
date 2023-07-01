
namespace Platformer
{
    public interface IPlayerAnimations
    {
        float Idle();
        float Walk();
        float JumpRising();
        float JumpFalling();
        float Dying();
        float Attack();
        float AirAttack();
        float Sit();
        float SitAttack();
        float RollDown();
        float Crouch();
        float DamageTaken();
        float SitDamageTaken();
        float Stunned();
    }
}