using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BearCubTrampoline : MonoBehaviour
    {
        [SerializeField]
        private float JumpForce;

        [SerializeField]
        private float Cooldown;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var cub = collision.GetComponentInParent<BearCub>();
            
            if (cub != null)
            {
                cub.TryJump(JumpForce, Cooldown);
            }
        }
    }
}