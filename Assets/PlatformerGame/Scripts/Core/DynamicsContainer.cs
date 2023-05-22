using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DynamicsContainer : MonoBehaviour, IDynamicsContainer
    {
        public Transform Transform => transform;
        private List<GameObject> Content;

        public void AddItem(GameObject entity)
        {
            if (!Content.Contains(entity))
            {
                Content.Add(entity);
            }
        }

        public void DeactivateAll()
        {
            foreach (var item in Content)
            {
                item.SetActive(false);
            }
        }

        private void Awake()
        {
            Content = new List<GameObject>();
        }
    }
}