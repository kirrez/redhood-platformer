using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class Defeat
    {
        private Megafrog Megafrog;

        private float Timer;
        private int BlastCount = 12;

        public Defeat(Megafrog megafrog)
        {
            Megafrog = megafrog;
        }

        private void SetState(Megafrog.State state)
        {
            Megafrog.SetState(state);
        }

        public void Start()
        {
            Megafrog.Rage = 0;
            Megafrog.FrogAnimator.PlayAttack();
            Megafrog.FrogAnimator.StartEndlessBlinking();
            Megafrog.HitBox.Hide();

            Megafrog.DynamicsContainer.DeactivateMain();
            Megafrog.DisableBodyDamage();

            var game = CompositionRoot.GetGame();
            Game.FadeScreen.FadeOut(Color.white, 2f);

            Timer = 0.25f;
            Megafrog.AudioManager.PlayMusic(EMusic.Boss_victory);

            SetState(Burning);
        }

        public void Burning()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 0.25f;

            var instance = Megafrog.ResourceManager.GetFromPool(GFXs.FireBlast);
            instance.transform.SetParent(Megafrog.DynamicsContainer.Temporary, false); // temporary container
            Megafrog.DynamicsContainer.AddTemporary(instance);

            Vector2 effectPosition = Megafrog.Body.transform.position + new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(0f, 3.5f));
            BaseGFX effect = instance.GetComponent<BaseGFX>();
            effect.Initiate(effectPosition, 1f);

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
                Timer = 1f;
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