using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface INavigation
    {
        void LoadStage(EStages stage);
        void SetLocation(int locationIndex, int spawnPointIndex, int confinerIndex, int musicIndex = 0);
    }
}