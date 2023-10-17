using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DeathFlameEffect : BaseGFX
    {
        public void Initiate(Vector2 newPosition, Vector2 newSize, float direction = 1)
        {
            base.Initiate(newPosition, direction);
            Renderer.size = newSize;
        }
    }
}