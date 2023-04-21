using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FlowEffector : MonoBehaviour
    {
        //[SerializeField]
        //private Vector2 Force;
        private Vector3 Speed = new Vector3(-2f, 0.2f, 0f);
        
        private Rigidbody2D PlayerBody;

        private void FixedUpdate()
        {
            if (PlayerBody == null) return;

            PlayerBody.AddForce(Speed, ForceMode2D.Force);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerBody = collision.gameObject.GetComponent<Rigidbody2D>();
            }
        }
    }
}