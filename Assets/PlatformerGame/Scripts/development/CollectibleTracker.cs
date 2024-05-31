using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [System.Serializable]
    public struct ItemProperty
    {
        public string Name;
        public int FirstIndex;
        public int LastIndex;
        [Header("Nubmer of items in level")]
        public int Amount;
    }

    [System.Serializable]
    public struct MoneyItemProperty
    {
        public string Name;
        public int FirstIndex;
        public int LastIndex;
        [Header("Nubmer of items in level")]
        public int Amount;
        [Header("Total sum of money")]
        public int Sum;
    }

    public class CollectibleTracker : MonoBehaviour
    {
        public List<MoneyItemProperty> MoneyItems;
        [Header("Total Money in Level :")]
        public int Money;
        public List<ItemProperty> Items;
    }
}