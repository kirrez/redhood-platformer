using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class Appearance
    {
        private IResourceManager ResourceManager;
        private Megafrog Megafrog;

        private float Timer;
        private float JumpForce = 1300f;
        private bool SplashTriggered;

        private void SetState(Megafrog.State state)
        {
            Megafrog.SetState(state);
        }

        public Appearance(Megafrog megafrog)
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            Megafrog = megafrog;
        }

        public void StartFromAbove()
        {
            LaunchHeavySpikedBalls();
            Megafrog.Body.isKinematic = true;//freeze

            var chance = UnityEngine.Random.Range(0, 6);
            int[] choises = { 0, 0, 1, 1, 2, 2, };

            Megafrog.Body.transform.position = Megafrog.TopRow[choises[chance]].position;

            Megafrog.FacePlayer();
            Megafrog.SetMask("EnemySolid");
            Megafrog.SetAnimation(Megafrog.JumpFall);
            Timer = 0.1f;

            SetState(RestBeforeFall);
        }

        private void RestBeforeFall()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Megafrog.Body.isKinematic = false; //unfreeze
            Megafrog.Body.velocity = Vector2.zero;

            SetState(JumpFalling);
        }

        private void JumpFalling()
        {
            if (Megafrog.IsGrounded(LayerMasks.Solid))
            {
                Megafrog.SetAnimation(Megafrog.Idle);
                Megafrog.Phase++;
                Timer = 1f;

                SetState(RestFinal);
            }
        }

        private void RestFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            SetState(Megafrog.DecidePhase);
        }

        public void StartFromBeneath()
        {
            LaunchHeavySpikedBalls();
            SplashTriggered = false;

            var chance = UnityEngine.Random.Range(0, 6);
            int[] choises = { 0, 0, 1, 1, 2, 2, };

            Megafrog.Body.transform.position = Megafrog.BottomRow[choises[chance]].position;

            Megafrog.FacePlayer();
            Megafrog.SetMask("EnemyTransparent");
            Megafrog.SetAnimation(Megafrog.JumpRise);

            Megafrog.UpdateLastBodyPosition();

            Megafrog.Body.isKinematic = true; //freeze
            Timer = 1.5f;

            SetState(RestBeforeJump);
        }

        public void StartFirstTime()
        {
            Megafrog.Show();

            Megafrog.Body.transform.position = Megafrog.BottomRow[0].position;

            Megafrog.FacePlayer();
            Megafrog.SetMask("EnemyTransparent");
            Megafrog.SetAnimation(Megafrog.JumpRise);
            Megafrog.HitBox.Hide();

            Megafrog.UpdateLastBodyPosition();

            Megafrog.Body.isKinematic = true;
            Timer = 2f;

            //Megafrog.Hitbox.SetActive(false);
            
            SetState(RestBeforeJump);
        }

        private void RestBeforeJump()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Megafrog.Body.isKinematic = false; //unfreeze
            Megafrog.Body.AddForce(new Vector2(0f, JumpForce));

            SetState(JumpRising);
        }

        private void JumpRising()
        {
            // add splash effect here )) (and in first time)
            if (!SplashTriggered && Megafrog.Body.transform.position.y + 1.5f > Megafrog.WaterLevel.position.y)
            {
                SplashTriggered = true;

                LaunchSplash(0f);
                LaunchSplash(-1.5f);
                LaunchSplash(-3.2f);
                LaunchSplash(1.5f);
                LaunchSplash(3.2f);
            }

            if (Megafrog.DeltaY < 0)
            {
                Megafrog.SetMask("EnemySolid");
                Megafrog.SetAnimation(Megafrog.JumpFall);

                SetState(JumpFalling);
            }
        }

        private void LaunchSplash(float offsetX)
        {
            var instance = ResourceManager.GetFromPool(GFXs.BlueSplash);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            var position = Megafrog.WaterLevel.position;
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);
            position.x = Megafrog.Body.transform.position.x + offsetX;
            instance.transform.position = position;
        }

        private void LaunchHeavySpikedBalls()
        {
            var instance = ResourceManager.GetFromPool(Enemies.HeavySpikedBall);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            var marks = Megafrog.Marks;
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);

            instance.transform.position = marks[0].position;
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 25f);
            instance.GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);

            instance = ResourceManager.GetFromPool(Enemies.HeavySpikedBall);
            dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);

            instance.transform.position = marks[1].position;
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 25f);
            instance.GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);
        }
    }
}