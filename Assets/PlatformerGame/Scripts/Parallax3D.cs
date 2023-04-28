using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Parallax3D : MonoBehaviour
    {
        [SerializeField]
        private float ScrollSpeed = 5f;

        [SerializeField]
        private Renderer Renderer;

        private void Update()
        {
            float offset = Time.time * ScrollSpeed;
            Renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}