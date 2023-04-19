using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public struct AnimationProperties
    {
        public List<Sprite> Animations;
        public float FramesPerSecond;
        public bool Loop;
        public bool Full;
    }
}