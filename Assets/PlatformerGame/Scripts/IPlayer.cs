using UnityEngine;
using System;

namespace Platformer
{
    public interface IPlayer
    {
        //Components
        Health Health { get; }
        IPlayerAnimations Animations { get; }

        event Action Interaction;

        Transform Transform { get; }
        Vector3 Position { get; set; }

        //Inputs
        float Horizontal { get; set; }
        float Vertical { get; set; }
        bool HitJump { get; set; }
        bool HitAttack { get; set; }
        float DeltaY { get; }

        float JumpDownTime { get; }
        float RollDownTime { get; }
        float DeathShockTime { get; }

        //Checks
        bool Grounded(LayerMask mask);
        bool Ceiled(LayerMask mask);
        bool StandingCeiled(LayerMask mask);
        float DirectionCheck();
        void AttackCheck();

        //Commands

        void SetState(EPlayerStates state, float time = 0f);
        void Walk(bool platform = false);
        void Crouch(bool platform = false);
        void StickToPlatform();
        void ResetVelocity();
        void PushDown();
        void Jump();
        void JumpDown(bool state);
        void InactivateCollider();
        void DamagePushBack();
        void RollDown();
        void StandUp();
        void SitDown();


        void ChangeHealthUI();
        void GetInput();
        void UpdateStateName(string name);
        void EnableGameOver();
    }
}