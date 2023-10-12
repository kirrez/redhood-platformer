using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class Defeat
    {
        private IResourceManager ResourceManager;
        private Megafrog Megafrog;

        private float Timer;
        private int BlastCount = 10;

        public Defeat(Megafrog megafrog)
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Megafrog = megafrog;
        }

        private void SetState(Megafrog.State state)
        {
            Megafrog.SetState(state);
        }

        public void Start()
        {
            Megafrog.Rage = 0;
            Megafrog.SetAnimation(EAnimations.AttackDamaged);
            Megafrog.HitBox.Hide();

            var dynamics = CompositionRoot.GetDynamicsContainer();
            dynamics.DeactivateAll();
            Megafrog.DisableBodyDamage();

            var game = CompositionRoot.GetGame();
            game.FadeScreen.FadeOut(Color.white, 2f);

            Timer = 0.3f;
            SetState(Burning);
        }

        public void Burning()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            //Instantiate new blast here

            SetState(Finish);
        }

        public void Finish()
        {
            BlastCount--;

            if (BlastCount > 0)
            {
                Timer = 0.3f;
                SetState(Burning);
            }

            if (BlastCount == 0)
            {
                Timer = 1.5f;
                SetState(RestFinal);
            }
        }

        public void RestFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Megafrog.Hide();
            Megafrog.MegafrogFight.WinBattle();
        }
    }
}