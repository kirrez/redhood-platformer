using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.MegafrogBoss
{
    public class MegafrogAnimator : MonoBehaviour
    {
        [SerializeField]
        private Sprite Idle;

        [SerializeField]
        private Sprite JumpRise;

        [SerializeField]
        private Sprite JumpFall;

        [SerializeField]
        private Sprite Attack;

        [SerializeField]
        private List<Sprite> AttackDamaged;

        [SerializeField]
        float AttackDamaged_fps;

        private SpriteRenderer Renderer;

        private delegate void State();
        State PlayAnimation = () => { };

        private float Timer;
        private int SpriteIndex;

        public void Initiate(SpriteRenderer renderer)
        {
            Renderer = renderer;
        }

        public void SetAnimation(EAnimations animation)
        {
            switch (animation)
            {
                case EAnimations.Idle:
                    Renderer.sprite = Idle;
                    PlayAnimation = PlayNone;
                    break;

                case EAnimations.JumpRise:
                    Renderer.sprite = JumpRise;
                    PlayAnimation = PlayNone;
                    break;

                case EAnimations.JumpFall:
                    Renderer.sprite = JumpFall;
                    PlayAnimation = PlayNone;
                    break;

                case EAnimations.Attack:
                    Renderer.sprite = Attack;
                    PlayAnimation = PlayNone;
                    break;

                case EAnimations.AttackDamaged:
                    Timer = AttackDamaged_fps;
                    SpriteIndex = 0;
                    PlayAnimation = PlayAttackDamaged;
                    break;
                default:
                    break;
            }
        }

        private void PlayNone()
        {

        }

        private void PlayAttackDamaged()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0) return;

            SpriteIndex++;

            if (SpriteIndex == AttackDamaged.Count)
            {
                SpriteIndex = 0;
            }

            Renderer.sprite = AttackDamaged[SpriteIndex];
            Timer = AttackDamaged_fps;
        }

        private void Update()
        {
            PlayAnimation();
        }
        
    }

}