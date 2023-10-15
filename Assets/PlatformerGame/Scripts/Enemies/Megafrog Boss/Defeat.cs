using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class Defeat
    {
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;
        private Megafrog Megafrog;

        private float Timer;
        private int BlastCount = 18;

        public Defeat(Megafrog megafrog)
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
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

            DynamicsContainer.DeactivateAll();
            Megafrog.DisableBodyDamage();

            var game = CompositionRoot.GetGame();
            game.FadeScreen.FadeOut(Color.white, 2f);

            Timer = 0.25f;
            SetState(Burning);
        }

        public void Burning()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;
            Timer = 0.25f;

            var instance = ResourceManager.GetFromPool(GFXs.FireBlast);
            instance.transform.SetParent(DynamicsContainer.Transform, false);
            DynamicsContainer.AddItem(instance);

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