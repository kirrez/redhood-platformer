using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherParticle : MonoBehaviour
{
    [SerializeField]
    private float MinAmplitude = -1f;

    [SerializeField]
    private float MaxAmplitude = 30f;

    private float Amplitude;

    private void OnEnable()
    {
        if (MinAmplitude >= 0)
        {
            Amplitude = Random.Range(MinAmplitude, MaxAmplitude);
        }
        else
        {
            Amplitude = MaxAmplitude;
        }

        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(Random.insideUnitCircle.normalized * Amplitude, ForceMode2D.Impulse);

        var chance = Random.Range(0f, 1f);
        if (chance < 0.5f) rigidbody.angularVelocity = Random.Range(100f, 200f);
        if (chance >= 0.5f) rigidbody.angularVelocity = Random.Range(100f, 200f) * -1f;
    }

    public void Initiate(Vector2 newPosition)
    {
        transform.position = newPosition;
    }
}
