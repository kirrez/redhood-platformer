using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CursedObstacle : MonoBehaviour
    {
        public delegate void Effect();
        public Effect Dispel;
    }
}