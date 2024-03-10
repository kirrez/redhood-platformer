using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HolyWaterWeapon : BaseGFX
    {
        protected DamageDealer DamageDealer;

        protected override void Awake()
        {
            base.Awake();
            DamageDealer = GetComponent<DamageDealer>();

            DamageDealer.Destructed -= OnDestructed;
            DamageDealer.Destructed += OnDestructed;
        }

        protected virtual void OnDestructed()
        {
            var sparkleEffect = ResourceManager.GetFromPool(GFXs.HolyWaterDisappear);
            sparkleEffect.transform.position = transform.position;
            DynamicsContainer.AddMain(sparkleEffect);
            AudioManager.PlaySound(ESounds.BottleCrush1);

            gameObject.SetActive(false);
        }

        protected override void OnDisappear()
        {
            base.OnDisappear();
        }
    }
}