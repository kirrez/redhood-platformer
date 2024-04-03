using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class MultipleShots
    {
        private IResourceManager ResourceManager;

        private Megafrog Megafrog;
        private int ShotCount;
        private float Timer;
        private float ShotForce = 13f;

        public MultipleShots(Megafrog megafrog)
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
            Megafrog.FacePlayer();
            Megafrog.FrogAnimator.PlayAttack();
            Megafrog.HitBox.Show();
            ShotCount = 5;

            SetState(Shoot);
        }

        private void Shoot()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            var instance = ResourceManager.GetFromPool(Enemies.BubbleBullet);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            //instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddMain(instance);
            instance.transform.position = Megafrog.FirePoint.position;

            var newPos = Megafrog.Player.Position;
            newPos.y = newPos.y + 0.5f;
            instance.GetComponent<Rigidbody2D>().velocity = (newPos - Megafrog.FirePoint.position).normalized * ShotForce;

            Timer = 1f;

            SetState(Finish);
        }

        private void Finish()
        {
            ShotCount--;
            if (ShotCount > 0)
            {
                SetState(Shoot);
            }
            if (ShotCount <= 0)
            {
                Megafrog.Phase = 3;
                Timer = 2f;

                SetState(RestVulnerable);
            }
        }

        private void RestVulnerable()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 0.5f;
            Megafrog.FrogAnimator.PlayIdle();
            Megafrog.HitBox.Hide();

            SetState(RestFinal);
        }

        private void RestFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            SetState(Megafrog.DecidePhase);
        }
    }
}