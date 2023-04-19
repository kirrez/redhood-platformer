using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Version for Animation

namespace Platformer
{
    public class PlayerAnimations2 : IPlayerAnimations
    {
        private Animation Animation;
        private RedHoodAnimations CurrentType;

        public PlayerAnimations2(Animation animation)
        {
            Animation = animation;
            var resourceManager = CompositionRoot.GetResourceManager();

            var types = Enum.GetValues(typeof(RedHoodAnimations));

            foreach (var animationType in types)
            {
                var type = (RedHoodAnimations)animationType;
                var instance = resourceManager.CreatePrefab<Animation, RedHoodAnimations>(type);
                var clip = instance.GetComponent<Animation>().clip;
                Animation.AddClip(clip, clip.name);
            }

            CurrentType = (RedHoodAnimations)types.GetValue(0);
        }

        private float PlayClip(RedHoodAnimations type)
        {
            CurrentType = type;
            var name = type.ToString();
            //Debug.Log(name);
            Animation.clip = Animation[name].clip;
            Animation.Play();

            //var length = Animation.GetClip(name).length;
            //return length;
            return 0f;
        }

        public float Idle()
        {
            var length = PlayClip(RedHoodAnimations.Idle);
            return length;
        }

        public float Walk()
        {
            var length = PlayClip(RedHoodAnimations.Walk);
            return length;
        }

        public float JumpRising()
        {
            var length = PlayClip(RedHoodAnimations.JumpRising);
            return length;
        }

        public float JumpFalling()
        {
            var length = PlayClip(RedHoodAnimations.JumpFalling);
            return length;
        }

        public float Dying()
        {
            var length = PlayClip(RedHoodAnimations.Dying);
            return length;
        }

        public float Attack()
        {
            var length = PlayClip(RedHoodAnimations.Attack);
            return length;
        }

        public float AirAttack()
        {
            var length = PlayClip(RedHoodAnimations.AirAttack);
            return length;
        }

        public float Sit()
        {
            var length = PlayClip(RedHoodAnimations.Sit);
            return length;
        }

        public float SitAttack()
        {
            var length = PlayClip(RedHoodAnimations.SitAttack);
            return length;
        }

        public float RollDown()
        {
            var length = PlayClip(RedHoodAnimations.RollDown);
            return length;
        }

        public float Crouch()
        {
            var length = PlayClip(RedHoodAnimations.Crouch);
            return length;
        }

        public float DamageTaken()
        {
            var length = PlayClip(RedHoodAnimations.DamageTaken);
            return length;
        }

        public float SitDamageTaken()
        {
            var length = PlayClip(RedHoodAnimations.DamageTaken);
            return length;
        }
    }
}