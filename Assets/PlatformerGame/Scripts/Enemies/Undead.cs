using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Undead : MonoBehaviour
    {
        public delegate void Effect(float duration);
        public Effect Freezing;
    }
}