using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float bleedChance = .5f;
    
    [SerializeField] private float horizontalForce = 500f;
    [SerializeField] private float verticalForce = 0f;
    [SerializeField] private float forceTorque = 0f;

    public void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health != null)
        {
            Debug.Log("pointy spike poked health");
            health.ChangeHP(-1);

            float random = UnityEngine.Random.Range(0f, 1f);
            if (random <= bleedChance)
            {
                health.AddStatus(HealthStatus.Bleeding);
                Debug.Log("pointy spike added bleeding");
            }
        }

        RigidBodyController controller = other.GetComponentInParent<RigidBodyController>();
        if (controller != null)
        {
            controller.HitInfo(transform, horizontalForce, verticalForce, forceTorque);
            Debug.Log("pointy spike pushed away");
        }
    }
}