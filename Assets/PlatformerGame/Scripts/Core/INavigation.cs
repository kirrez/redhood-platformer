using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public interface INavigation
    {
        event Action ChangingCheckpoint;
        void LoadStage(EStages stage);
        void SetLocation(int locationIndex, int spawnPointIndex, int confinerIndex, int musicIndex = 0);
        void ChangeCheckpoint();
    }
}