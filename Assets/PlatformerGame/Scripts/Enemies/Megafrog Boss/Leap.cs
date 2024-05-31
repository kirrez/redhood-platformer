using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class Leap
    {
        private Megafrog Megafrog;
        private float Timer;
        private float LeapHorizontal = 418f;
        private float LeapVertical = 650f;
        private float ShotForce = 13f;
        private float DirectionX;

        private int FrogArea;
        private int PlayerArea;
        private int TargetArea;

        private void SetState(Megafrog.State state)
        {
            Megafrog.SetState(state);
        }

        public Leap(Megafrog megafrog)
        {
            Megafrog = megafrog;
        }

        public void StartLeap()
        {
            DefinePlayerArea();
            DefineFrogArea();
            DefineLeapTargetArea();

            //new DirectionX
            if (FrogArea < TargetArea) DirectionX = 1f;
            if (FrogArea > TargetArea) DirectionX = -1f;
            Megafrog.SetDirectionX(DirectionX);
            Megafrog.ChangeDirection();

            Megafrog.Body.AddForce(new Vector2(LeapHorizontal * DirectionX, LeapVertical));
            Megafrog.FrogAnimator.PlayJumpRise();

            //
            Megafrog.AudioManager.PlaySound(ESounds.MFrog_Jump);
            SetState(JumpRising);
        }

        public void StartShot()
        {
            Megafrog.FacePlayer();
            Megafrog.FrogAnimator.PlayAttack();
            Megafrog.HitBox.Show();
            Timer = 1.5f;

            var instance = Megafrog.ResourceManager.GetFromPool(Enemies.BubbleBullet);
            Megafrog.DynamicsContainer.AddMain(instance);

            instance.transform.position = Megafrog.FirePoint.position;

            var newPos = Megafrog.Player.Position;
            newPos.y = newPos.y + 0.5f;
            instance.GetComponent<Rigidbody2D>().velocity = (newPos - Megafrog.FirePoint.position).normalized * ShotForce;

            //
            Megafrog.AudioManager.PlaySound(ESounds.BulletShot1);
            SetState(FinishShot);
        }

        private void FinishShot()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Megafrog.FrogAnimator.PlayIdle();
            Megafrog.HitBox.Hide();

            SetState(StartLeap);
        }

        private void JumpRising()
        {
            if (Megafrog.DeltaY < 0)
            {
                Megafrog.SetMask(LayerNames.EnemySolid);
                Megafrog.FrogAnimator.PlayJumpFall();

                SetState(JumpFalling);
            }
        }

        private void JumpFalling()
        {
            if (Megafrog.IsGrounded(LayerMasks.Solid))
            {
                Megafrog.Phase++;
                Megafrog.FrogAnimator.PlayIdle();
                Timer = 1f;

                if (Megafrog.Phase == 3)
                {
                    SetState(RestBeforeQuack);
                    return;
                }

                //
                Megafrog.AudioManager.PlaySound(ESounds.MFrog_Landing);
                SetState(RestFinal);
            }
        }

        private void RestBeforeQuack()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Megafrog.FrogAnimator.PlayAttack();
            Megafrog.HitBox.Show();
            Timer = 1.5f;

            //
            Megafrog.AudioManager.PlaySound(ESounds.MFrog_Quack);
            SetState(Quack);
        }

        private void Quack()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            Megafrog.FrogAnimator.PlayIdle();
            Megafrog.HitBox.Hide();
            Timer = 1f;

            SetState(RestFinal);
        }

        private void RestFinal()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer > 0) return;

            SetState(Megafrog.DecidePhase);
        }

        private void DefinePlayerArea()
        {
            var playerX = Megafrog.Player.Position.x;
            var marks = Megafrog.Marks;

            if (playerX < marks[0].position.x)
            {
                PlayerArea = 0;
            }

            if (playerX >= marks[0].position.x && playerX <= marks[1].position.x)
            {
                PlayerArea = 1;
            }

            if (playerX > marks[1].position.x)
            {
                PlayerArea = 2;
            }
        }

        private void DefineFrogArea()
        {
            var frogX = Megafrog.Body.transform.position.x;
            var marks = Megafrog.Marks;

            if (frogX < marks[0].position.x)
            {
                FrogArea = 0;
            }

            if (frogX >= marks[0].position.x && frogX <= marks[1].position.x)
            {
                FrogArea = 1;
            }

            if (frogX > marks[1].position.x)
            {
                FrogArea = 2;
            }
        }

        public void DefineLeapTargetArea()
        {
            if (FrogArea < PlayerArea)
            {
                TargetArea = FrogArea + 1;
            }

            if (FrogArea > PlayerArea)
            {
                TargetArea = FrogArea - 1;
            }

            if (FrogArea == PlayerArea)
            {
                if (FrogArea == 0 || FrogArea == 2) TargetArea = 1;
                if (FrogArea == 1)
                {
                    if (Megafrog.Body.transform.position.x <= Megafrog.Player.Position.x)
                    {
                        TargetArea = 2;
                    }
                    else
                    {
                        TargetArea = 0;
                    }
                }
            }
        }
    }
}