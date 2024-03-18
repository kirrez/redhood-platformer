using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class Undead : MonoBehaviour
    {
        public Action Perished = () => { };

        public delegate void Effect(float duration);
        public Effect Freezing;

        protected void OnDisable()
        {
            Perished?.Invoke();
        }
    }
}