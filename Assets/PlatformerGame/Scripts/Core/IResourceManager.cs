using System;
using UnityEngine;

namespace Platformer
{
    public interface IResourceManager
    {
        //UnityEngine.Object GetAsset<T>(T type) where T : Enum;
        T CreatePrefab<T, E>(E type) where E : Enum;
        GameObject GetFromPool<E>(E objType) where E : Enum;
    }
}