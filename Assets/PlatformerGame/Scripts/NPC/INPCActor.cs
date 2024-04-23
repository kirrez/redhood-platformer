using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface INPCActor
    {
        void WatchPlayer(bool watch);
        void HideNPC();
    }
}