using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformWaypoint : MonoBehaviour
    {
        [SerializeField]
        private bool Teleport;

        //Teleport - is a point where platform arrives from previous point instantly. It's a destination point
        public bool IsTeleport()
        {
            return Teleport;
        }
    }
}