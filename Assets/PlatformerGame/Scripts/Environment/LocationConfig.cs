using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [System.Serializable]
    public struct LocationConfig
    {
        public List<GameObject> TargetLocation;
        public List<GameObject> OriginLocation;
        public PolygonCollider2D Confiner;
    }
}