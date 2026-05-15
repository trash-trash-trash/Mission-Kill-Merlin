using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Transform pointA;  
    public Transform pointB;  
    public LayerMask hitLayers;

    public Player player;
    public float shootCooldownTime;

    public LineRenderer line;
    
    public bool canShoot = true;
    public bool aiming = false;

    public ItemSO equippedItemSo;
    
    public void Equip(ItemSO aItemSo)
    { 
        //move gunpoint to range
        equippedItemSo = aItemSo;
        Vector3 localPos = pointB.localPosition;
        localPos.x = aItemSo.range;
        pointB.localPosition = localPos;
    }
    
    void ShowHitLine(Vector3 start, Vector3 end, bool input)
    {
        if (input)
        {
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.startColor = Color.red;
            line.endColor = Color.red;
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }

    public bool Equip()
    {
        throw new System.NotImplementedException();
    }

    public void Drop()
    {
        throw new System.NotImplementedException();
    }

    public void Aim()
    {
        aiming = true;
    }

    public void Use()
    {
        throw new System.NotImplementedException();
    }

    public void Throw()
    {
        throw new System.NotImplementedException();
    }

    public void Holster()
    {
        aiming = false;
    }

    public void Update()
    {
        if (aiming)
        {
            ShowHitLine(pointA.position, pointB.position,true);
        }
        else if (line.enabled)
            ShowHitLine(pointA.position, pointB.position, false);
    }

    //assumes only player will shoot...
    //aggroAction should pass along CharacterBase 
    void Shoot()
    {
        if (!canShoot)
            return; 
        if (pointA != null && pointB != null)
        {
            aiming = false;
            
            Vector3 direction = pointB.position - pointA.position; 
            float distance = direction.magnitude;

            RaycastHit hit;
            if (Physics.Raycast(pointA.position, direction.normalized, out hit, distance, hitLayers))
            {
                Health hp = hit.collider.gameObject.GetComponent<Health>();
                if (hp == null)
                    hp = hit.collider.gameObject.GetComponentInParent<Health>();
                if (hp != null)
                {
                    //hp.HitByWeapon(equippedItemSo);
                    Debug.Log("Shot "+hit.collider.gameObject.name); 
                    
                    Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (rb == null)
                        rb = hit.collider.gameObject.GetComponentInParent<Rigidbody>();
                    if (rb != null)
                    {
                        //add force
                        Vector3 weaponForceDir = (rb.position - hit.point).normalized;
                        Vector3 shoveVelocity = weaponForceDir * equippedItemSo.weaponForce;
                        rb.linearVelocity = shoveVelocity;

                        //add torque
                        Vector3 shoveDirection = (rb.position - hit.point).normalized;
                        Vector3 randomOffset = Random.insideUnitSphere * 0.2f;
                        Vector3 torqueAxis = Vector3.Cross(Vector3.up, shoveDirection + randomOffset).normalized;

                        rb.AddTorque(torqueAxis * equippedItemSo.weaponForceTorque, ForceMode.Impulse);
                    }
                }
                
               
            }
            
            StartCoroutine(ShootCooldownCoro());
        }
        
    }
    
    IEnumerator ShootCooldownCoro()
    { 
        canShoot = false;
        yield return new WaitForSeconds(shootCooldownTime);
        canShoot = true;
    }

    public void AggroAction()
    {
        Shoot();
    }

    public GameObject ReturnSelf()
    {
        return gameObject;
    }
}
