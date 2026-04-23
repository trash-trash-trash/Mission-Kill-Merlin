using UnityEngine;

public class RigidBodyController : MonoBehaviour
{
    public Rigidbody rb;
    
    public void HitInfo(Transform forceOrigin, float shoveHorizontalForce, float verticalForce, float forceTorque)
    {
        rb.constraints = RigidbodyConstraints.None;
                    
        //add force
        Vector3 direction = (rb.position - forceOrigin.position).normalized;
        Vector3 shoveVelocity = direction * shoveHorizontalForce;
        shoveVelocity.y = verticalForce;
        rb.linearVelocity = shoveVelocity;
                    
        //add torque
        Vector3 shoveDirection = (rb.position - forceOrigin.position).normalized;
        Vector3 randomOffset = Random.insideUnitSphere * 0.2f;
        Vector3 torqueAxis = Vector3.Cross(Vector3.up, shoveDirection + randomOffset).normalized;

        if(forceTorque > 0)
            rb.AddTorque(torqueAxis * forceTorque, ForceMode.Impulse);
    }
}
