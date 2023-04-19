using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ParallaxController : MonoBehaviour
    {
        [SerializeField]
        private Transform[] Layers;

        [SerializeField]
        private float[] Offset;

        private void Update()
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i].position = transform.position * Offset[i];
            }
        }
    }
}