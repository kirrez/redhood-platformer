using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animator Version, not in use anymore

namespace Platformer
{
    public class PlayerAnimations : IPlayerAnimations
    {
        private Animator Animator;

        public PlayerAnimations(Animator animator)
        {
            Animator = animator;
        }

        public float Idle()
        {
            Animator.SetInteger("State", 0);
            return 0f;
        }

        public float Walk()
        {
            Animator.SetInteger("State", 1);
            return 0f;
        }

        public float JumpRising()
        {
            Animator.SetInteger("State", 2);
            return 0f;
        }

        public float JumpFalling()
        {
            Animator.SetInteger("State", 3);
            return 0f;
        }

        public float Dying()
        {
            Animator.SetTrigger("Dead");
            return 0f;
        }

        public float Attack()
        {
            Animator.SetInteger("State", 4);
            return 0f;
        }

        public float AirAttack()
        {
            Animator.SetInteger("State", 5);
            return 0f;
        }

        public float Sit()
        {
            Animator.SetInteger("State", 6);
            return 0f;
        }

        public float SitAttack()
        {
            Animator.SetInteger("State", 7);
            return 0f;
        }

        public float RollDown()
        {
            Animator.SetInteger("State", 8);
            return 0f;
        }

        public float Crouch()
        {
            Animator.SetInteger("State", 9);
            return 0f;
        }

        public float DamageTaken()
        {
            Animator.SetInteger("State", 10);
            return 0f;
        }

        public float SitDamageTaken()
        {
            Animator.SetInteger("State", 10);
            return 0f;
        }
    }
}