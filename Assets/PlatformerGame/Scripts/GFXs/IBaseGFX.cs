using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IBaseGFX
    {
        public void Initiate(Vector2 newPosition, float direction = 1f);
    }
}