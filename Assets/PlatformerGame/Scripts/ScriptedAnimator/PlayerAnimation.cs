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

        public float Moving()
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

        public float Crouch()
        {
            return Animator.Sit();
        }

        public float CrouchAttack()
        {
            return Animator.SitAttack();
        }

        public float RollDown()
        {
            return Animator.RollDown();
        }

        public float CrouchMoving()
        {
            return Animator.Crouch();
        }

        public float DamageTaken()
        {
            return Animator.DamageTaken();
        }

        public float CrouchDamageTaken()
        {
            return Animator.SitDamageTaken();
        }

        public float Stunned()
        {
            return Animator.Stunned();
        }
    }
}