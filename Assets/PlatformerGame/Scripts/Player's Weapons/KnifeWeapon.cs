using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class KnifeWeapon : BaseGFX
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
            gameObject.SetActive(false);
        }

        protected override void OnDisappear()
        {
            base.OnDisappear();
        }
    }
}