using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shove : MonoBehaviour
{
    [Header("Shove Settings")]
    //atm origin is player body, rotation is cameraArm
    public Transform shoveOrigin;
    
    public Vector3 boxHalfExtents = new Vector3(1, 1, 1);
    public Vector3 boxOffset = Vector3.forward;
    public float shoveHorizontalForce = 10f;
    public float shoveVerticalForce = 5f;
    public float shoveTorque = 5f;
    public LayerMask shoveLayer;

    public bool pushedSomeone;
    
    public bool canShove = true;
    public float shoveCooldown = 0.5f;

    public void TryShove(Transform newShoveOrigin)
    {
        if (!canShove)
            return;
        
        shoveOrigin = newShoveOrigin;
        
            //se the Y rotation for forward-facing direction
            Quaternion yRotation = Quaternion.Euler(0f, shoveOrigin.rotation.eulerAngles.y + 90, 0f);

            //offset by z
            Vector3 forwardOffset = yRotation * Vector3.forward * boxHalfExtents.z;
            Vector3 center = shoveOrigin.position + forwardOffset;

            Collider[] hits = Physics.OverlapBox(center, boxHalfExtents, yRotation, shoveLayer);

            pushedSomeone = false;

            foreach (Collider hit in hits)
            {
                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null && rb != GetComponentInChildren<Rigidbody>())
                {
                    Health hp = rb.gameObject.GetComponent<Health>();
                    if(hp!=null)
                        hp.ChangeHP(0);

                    rb.constraints = RigidbodyConstraints.None;
                    
                    //add force
                    Vector3 direction = (rb.position - shoveOrigin.position).normalized;
                    Vector3 shoveVelocity = direction * shoveHorizontalForce;
                    shoveVelocity.y = shoveVerticalForce;
                    rb.linearVelocity = shoveVelocity;
                    
                    //add torque
                    Vector3 shoveDirection = (rb.position - shoveOrigin.position).normalized;
                    Vector3 randomOffset = Random.insideUnitSphere * 0.2f;
                    Vector3 torqueAxis = Vector3.Cross(Vector3.up, shoveDirection + randomOffset).normalized;

                    rb.AddTorque(torqueAxis * shoveTorque, ForceMode.Impulse);

                    Debug.Log("Shoved " + rb.gameObject.name);
                    if (!pushedSomeone)
                        pushedSomeone = true;
            }
            StartCoroutine(ShoveCooldownCoro());
        }
    }

    IEnumerator ShoveCooldownCoro()
    {
        canShove = false;
        yield return new WaitForSeconds(shoveCooldown);
        canShove = true;
        pushedSomeone = false;
    }
}
