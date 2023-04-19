using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Now works with PlayerScriptedAnimations

namespace Platformer.ScriptedAnimator
{
    public class PlayerAnimationsTest : IPlayerAnimations
    {
        private PlayerScriptedAnimations Animation;

        public PlayerAnimationsTest(PlayerScriptedAnimations animation)
        {
            Animation = animation;
        }

        public float Idle()
        {
            return Animation.Idle();
        }

        public float Walk()
        {
            return Animation.Walk();
        }

        public float JumpRising()
        {
            return Animation.JumpRising();
        }

        public float JumpFalling()
        {
            return Animation.JumpFalling();
        }

        public float Dying()
        {
            return Animation.Dying();
        }

        public float Attack()
        {
            return Animation.Attack();
        }

        public float AirAttack()
        {
            return Animation.AirAttack();
        }

        public float Sit()
        {
            return Animation.Sit();
        }

        public float SitAttack()
        {
            return Animation.SitAttack();
        }

        public float RollDown()
        {
            return Animation.RollDown();
        }

        public float Crouch()
        {
            return Animation.Crouch();
        }

        public float DamageTaken()
        {
            return Animation.DamageTaken();
        }

        public float SitDamageTaken()
        {
            return Animation.SitDamageTaken();
        }
    }
}