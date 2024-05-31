using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class ToadRain
    {
        private Megafrog Megafrog;

        private int DropStartOffset = 2;
        private int LaunchCount;
        private int DropCount;

        private float Timer;

        public ToadRain(Megafrog megafrog)
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
            Megafrog.HitBox.Show();
            LaunchCount = 10;
            DropCount = 30;

            //initial sound, like horn or something
            Megafrog.AudioManager.PlaySound(ESounds.MFrog_CreepyScreech);
            SetState(LaunchToads);
        }

        private void LaunchToads()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            if (LaunchCount == 9 || LaunchCount == 5 || DropCount == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    var instance = Megafrog.ResourceManager.GetFromPool(Enemies.ToadLaunch);
                    Megafrog.DynamicsContainer.AddTemporary(instance);
                    instance.GetComponent<ToadLaunch>().Launch(Megafrog.WaterLevel.position);
                }

                Megafrog.AudioManager.PlaySound(ESounds.Splash2);
            }
            else if (LaunchCount > 0) 
            {
                var instance = Megafrog.ResourceManager.GetFromPool(Enemies.ToadLaunch);
                Megafrog.DynamicsContainer.AddTemporary(instance);
                instance.GetComponent<ToadLaunch>().Launch(Megafrog.WaterLevel.position);

                Megafrog.AudioManager.PlaySound(ESounds.Splash2);
            }

            if (LaunchCount < DropStartOffset)
            {
                var instance = Megafrog.ResourceManager.GetFromPool(Enemies.ToadDrop);
                Megafrog.DynamicsContainer.AddTemporary(instance);
                instance.GetComponent<ToadDrop>().Initiate(Megafrog.ToadRainLevel.position);
                
                if (DropCount % 3 == 0)
                {
                    instance = Megafrog.ResourceManager.GetFromPool(Enemies.ToadDrop);
                    Megafrog.DynamicsContainer.AddTemporary(instance);
                    instance.GetComponent<ToadDrop>().Initiate(Megafrog.ToadRainLevel.position);

                    instance = Megafrog.ResourceManager.GetFromPool(Enemies.ToadDrop);
                    Megafrog.DynamicsContainer.AddTemporary(instance);
                    instance.GetComponent<ToadDrop>().Initiate(Megafrog.ToadRainLevel.position);
                }
            }

            Timer = 0.2f;

            SetState(Finish);
        }

        private void Finish()
        {
            LaunchCount--;

            if (LaunchCount < DropStartOffset)
            {
                DropCount--;
            }

            if (LaunchCount > 0 || DropCount > 0)
            {
                SetState(LaunchToads);
                return;
            }

            if (LaunchCount <= 0 && DropCount <= 0)
            {
                Timer = 3f;
                Megafrog.Phase = 3;
                Megafrog.Rage = 0;

                SetState(RestVulnerable);
            }
        }

        private void RestVulnerable()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Timer = 1f;
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