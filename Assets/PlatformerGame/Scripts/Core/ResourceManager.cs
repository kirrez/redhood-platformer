using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ResourceManager : IResourceManager
    {
        private List<PoolItem> ObjectPool = new List<PoolItem>();

        //public UnityEngine.Object GetAsset<TEnum>(TEnum resource)
        //    where TEnum : struct, IComparable, IConvertible, IFormattable
        //{
        //    var type = typeof(TEnum);
        //    var value = EnumInt32ToInt.Convert(resource);
        //    var name = resource.ToString();

        //    var asset = GetAsset(type, value, name);
        //    return asset;
        //}

        //private UnityEngine.Object GetAsset(Type type, int value, string name)
        //{
        //    var key = new EnumComparerKey(type, value);

        //    if (ResourceCache.ContainsKey(key) == false)
        //    {
        //        var path = GetPathFromNamespace(type, name);
        //        var asset = Resources.Load<UnityEngine.Object>(path);

        //        if (asset == null)
        //            throw new UnityException("Can't load resource '" + path + "'");

        //        ResourceCache[key] = asset;
        //    }

        //    return ResourceCache[key];
        //}

        //public UnityEngine.Object GetAsset<T>(T type)
        //    where T : Enum
        //{
        //    var path = type.GetType().Name + "/" + type.ToString();
        //    var asset = Resources.Load<UnityEngine.Object>(path);
            
        //    return asset;
        //}

        public T CreatePrefab<T, E>(E type)
            where E : Enum
        {
            var path = type.GetType().Name + "/" + type.ToString();
            var asset = Resources.Load<GameObject>(path);
            var instance = GameObject.Instantiate(asset);
            var component = instance.GetComponent<T>();

            return component;
        }

        public void RemoveFromPool()
        {

        }

        public GameObject GetFromPool<E>(E objType)
            where E : Enum
        {
            PoolItem unit;
            string path = "";
            unit.type = objType.GetType();
            unit.value = objType;

            if (ObjectPool.Count > 0)
            {
                foreach (PoolItem element in ObjectPool)
                {
                    if (element.type.Equals(unit.type) && element.value.Equals(unit.value) && element.item.activeInHierarchy == false)
                    {
                        unit.item = element.item;
                        unit.item.SetActive(true);
                        return unit.item;
                    }
                }
            }

            if (unit.type.Namespace != null)
            {
                path = unit.type.ToString();
                int length = unit.type.Namespace.Length;
                path = path.Remove(0, length + 1) + "/"; // removing namespace and "." char
            }
            else
            {
                path = unit.type.ToString();
            }
            path += unit.value.ToString();

            var asset = Resources.Load<GameObject>(path);
            var instance = GameObject.Instantiate(asset);

            unit.item = instance;
            ObjectPool.Add(unit);
            return unit.item;
        }
    }
}

