using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class PlayerAnimation : IPlayerAnimations
    {
        private PlayerAnimator Animator;

        public PlayerAnimation(PlayerAnimator animator)
        {
            Animator = animator;
        }
        public float Idle()
        {
            return Animator.Idle();
        }

        public float Walk()
        {
            return Animator.Walk();
        }

        public float JumpRising()
        {
            return Animator.JumpRising();
        }

        public float JumpFalling()
        {
            return Animator.JumpFalling();
        }

        public float Dying()
        {
            return Animator.Dying();
        }

        public float Attack()
        {
            return Animator.Attack();
        }

        public float AirAttack()
        {
            return Animator.AirAttack();
        }

        public float Sit()
        {
            return Animator.Sit();
        }

        public float SitAttack()
        {
            return Animator.SitAttack();
        }

        public float RollDown()
        {
            return Animator.RollDown();
        }

        public float Crouch()
        {
            return Animator.Crouch();
        }

        public float DamageTaken()
        {
            return Animator.DamageTaken();
        }

        public float SitDamageTaken()
        {
            return Animator.SitDamageTaken();
        }
    }
}