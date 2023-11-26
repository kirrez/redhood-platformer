using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class SpikeVomit
    {
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private Megafrog Megafrog;
        private int ShotCount;
        private float Timer;

        public SpikeVomit(Megafrog megafrog)
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
            Megafrog.FacePlayer();
            Megafrog.SetAnimation(FrogAnimations.Attack);
            Megafrog.HitBox.Show();
            Timer = 0.45f;
            ShotCount = 12;

            SetState(Shoot);
        }

        private void Shoot()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 0.45f;
            var instance = ResourceManager.GetFromPool(Enemies.SpikedBullet);
            //instance.transform.SetParent(DynamicsContainer.Transform, false);
            DynamicsContainer.AddTemporary(instance);
            instance.transform.position = Megafrog.FirePoint.position;

            if (ShotCount == 10 || ShotCount == 4)
            {
                instance.GetComponent<Rigidbody2D>().velocity = new Vector3(3.5f * Megafrog.DirectionX, 4f, 0f);
            }
            else
            {
                instance.GetComponent<Rigidbody2D>().velocity = new Vector3(UnityEngine.Random.Range(2f, 11f) * Megafrog.DirectionX, UnityEngine.Random.Range(4f, 12f), 0f);
            }
            instance.GetComponent<Rigidbody2D>().AddTorque(10f * -Megafrog.DirectionX, ForceMode2D.Impulse);

            SetState(Finish);
        }

        private void Finish()
        {
            ShotCount--;
            if (ShotCount > 0)
            {
                Megafrog.SetState(Shoot);
            }
            if (ShotCount <= 0)
            {
                Megafrog.Phase = 3;
                Timer = 2.5f;

                SetState(RestVulnerable);
            }
        }

        private void RestVulnerable()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 0.5f;
            Megafrog.SetAnimation(FrogAnimations.Idle);
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