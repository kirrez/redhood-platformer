using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BridgeShatter : MonoBehaviour
    {
        private void OnEnable()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 10f), 0f) * 25);
            rigidbody.AddTorque(Random.Range(-50f, 50f));
            
            Destroy(gameObject, 3f);
        }
    }
}