using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public abstract class BasePlatform : MonoBehaviour
    {
        [SerializeField]
        protected float Delay = 0.5f;

        protected float Timer = 0f;

        protected bool IsActive;
        protected bool Inside;

        protected abstract void FixedUpdate();

        public void ComeThrough()
        {
            Timer = Delay;
            gameObject.layer = (int)Layers.OneWayTransparent;
            IsActive = true;
        }
    }
}