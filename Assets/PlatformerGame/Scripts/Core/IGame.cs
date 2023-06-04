using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.UI;

namespace Platformer
{
    public interface IGame
    {
        public DialogueModel Dialogue { get; }
        void LoadStage(EStages stage);
        void SetLocation(int locationIndex = 0, int spawnPointIndex = 0);
    }
}