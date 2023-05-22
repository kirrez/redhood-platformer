using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IGame
    {
        void SetStage(Stage newStage);
    }
}