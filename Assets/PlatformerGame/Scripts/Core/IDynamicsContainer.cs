using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IDynamicsContainer
    {
        Transform Transform { get; }

        void AddItem(GameObject entity);
        void DeactivateAll();
    }
}