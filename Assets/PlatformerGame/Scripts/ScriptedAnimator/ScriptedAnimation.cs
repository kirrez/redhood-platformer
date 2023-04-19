using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public class ScriptedAnimation : MonoBehaviour
    {
        public List<Sprite> Animations;
        public float FramesPerSecond = 5f;
        public bool Loop = false;
        public bool Full = false;
    }
}