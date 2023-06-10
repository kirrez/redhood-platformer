using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.ScriptedAnimator
{
    public interface IDoorAnimator
    {
        void AnimateOpen();
        void AnimateClose();
    }
}