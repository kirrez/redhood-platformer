using UnityEngine;
using System;

namespace Platformer
{
    public interface IPlayer
    {
        event Action Interaction;

        Transform Transform { get; }
        Vector3 Position { get; set; }

        void Initiate(IGame game);
        void UpdateMaxLives();
        void Revive();

        void Stun(float time);

        //Refactoring
        void Idle();
        void Jump();
        void MoveLeft();
        void MoveRight();
        void Fire();
        void ExtraFire();
        void Use();
        void Crouch();
        void Stand();

        EFacing GetFacing();
    }
}