using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class WeakHolyWater : HolyWaterWeapon
    {
        protected override void OnDestructed()
        {
            var effect = ResourceManager.GetFromPool(GFXs.HolyWaterWave);
            var wave = effect.GetComponent<HolyWaterWave>();
            wave.Initiate(transform.position, 4f, 2.6f);
            DynamicsContainer.AddMain(effect);

            base.OnDestructed();
        }
    }
}