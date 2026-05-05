using System;
using UnityEngine;

public class ImpactDamageThreshold : MonoBehaviour
{
    public float speedThreshold = 5f;
    public LayerMask validLayers;

    public event Action<float> AnnounceHardImpact;

    public bool checkingForImpact = true;

    void OnCollisionEnter(Collision collision)
    {
        if (!checkingForImpact)
            return;
        
        // Check layer first
        if (((1 << collision.gameObject.layer) & validLayers) == 0)
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed >= speedThreshold)
        {
            Debug.Log("Bang");
            AnnounceHardImpact?.Invoke(impactSpeed);
        }
    }
}