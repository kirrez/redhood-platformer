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
            wave.Initiate(transform.position, 18, 3f);
            DynamicsContainer.AddMain(effect);
            
            wave.LaunchParticles();

            base.OnDestructed();
        }
    }
}