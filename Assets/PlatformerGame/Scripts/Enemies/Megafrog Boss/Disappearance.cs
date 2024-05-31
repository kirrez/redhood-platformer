using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class Disappearance
    {
        private Megafrog Megafrog;
        private float Timer;

        private float JumpForce = 1500f;

        private void SetState(Megafrog.State state)
        {
            Megafrog.SetState(state);
        }

        public Disappearance(Megafrog megafrog)
        {
            Megafrog = megafrog;
        }

        public void Start()
        {
            Megafrog.FacePlayer();

            Megafrog.Body.AddForce(new Vector2(0f, JumpForce));
            Megafrog.FrogAnimator.PlayJumpRise();

            Megafrog.AudioManager.PlaySound(ESounds.MFrog_Rise);

            SetState(JumpRising);
        }

        private void JumpRising()
        {
            if (Megafrog.DeltaY > 0) return;

            Megafrog.Body.isKinematic = true; //freeze Body?
            Megafrog.Body.velocity = Vector2.zero;
            Megafrog.Phase = 0;
            Timer = 0.2f;

            SetState(Rest);
        }

        private void Rest()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            SetState(Megafrog.DecidePhase);
        }
    }
}