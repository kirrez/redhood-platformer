using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IDynamicsContainer
    {
        Transform Enemies { get; }
        Transform Main { get; }
        Transform Music { get; }
        Transform Sounds { get; }
        Transform Temporary { get; }

        void AddEnemy(GameObject item);
        void AddMain(GameObject item);
        void AddMusic(GameObject item);
        void AddSound(GameObject item);
        void AddTemporary(GameObject item);

        void DeactivateEnemies();
        void DeactivateMain();
        void DeactivateTemporary();
    }
}