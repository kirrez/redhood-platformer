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

        bool HitInteraction { get; set; }
        float DeltaY { get; }

        float JumpDownTime { get; }
        float RollDownTime { get; }
        float DeathShockTime { get; }

        //Checks
        bool Grounded(LayerMask mask);
        bool Ceiled(LayerMask mask);
        bool StandingCeiled(LayerMask mask);
        float DirectionCheck();

        //Commands

        void Initiate(IGame game);
        void UpdateMaxLives();
        void Revive();

        void HoldByInteraction();
        void ReleasedByInteraction();
        void Stun(float time);

        //void SetState(EPlayerStates state, float time = 0f);
        //void SetDeltaY();
        //void Walk();
        //void Crouch();
        //void StickToPlatform();
        //void ResetVelocity();
        //void PushDown();
        //void Jump();
        //void JumpDown();
        //void ReleasePlatform();
        //void InactivateCollider(bool flag);
        //void DamagePushBack();
        //void RollDown();
        //void StandUp();
        //void SitDown();

        //Weapons
        //void UpdateAttackTimers();
        //bool IsKnifeAttack();
        //bool IsAxeAttack();
        //bool IsHolyWaterAttack();
        //void ShootKnife();
        //void ShootAxe();
        //void ShootHolyWater();

        //void ChangeHealthUI();
        //void GetInput();
        //void GetInteractionInput();
        //void EnableGameOver();
    }
}