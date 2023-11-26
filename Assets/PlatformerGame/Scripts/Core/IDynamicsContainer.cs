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

        public void AddEnemy(GameObject item);
        public void AddMain(GameObject item);
        public void AddMusic(GameObject item);
        public void AddSound(GameObject item);
        public void AddTemporary(GameObject item);

        public void DeactivateEnemies();
        public void DeactivateMain();
        public void DeactivateTemporary();
    }
}