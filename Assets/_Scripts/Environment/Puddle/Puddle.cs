using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct PuddleData
{
    public HealthStatus associatedStatus;
    public int secondsRemainingCharge;
    public float expiryTimerDecayRate;
    
    public int originalSecondsRemainingCharge;
}

public class Puddle : MonoBehaviour
{
    public PuddleData data;
    public LayerMask puddleToHitLayers;
    public LayerMask groundLayer;
    
    public event Action<Puddle> AnnounceSet;
    
    public event Action<Puddle> AnnounceExpired;

    public IEnumerator puddleShrinkCoro;

    public float groundOffset = 0;

    public void SetPuddleType(PuddleData newPuddleData)
    {
        data = newPuddleData;
        data.secondsRemainingCharge = data.originalSecondsRemainingCharge;
        AnnounceSet?.Invoke(this);
        puddleShrinkCoro = ShrinkPuddleOverTime();
        HackPlaceOnGround();
        StartPuddleDecay();
    }

    public void StartPuddleDecay()
    {
        if (puddleShrinkCoro != null)
        {
            StartCoroutine(puddleShrinkCoro);
        }
    }

    public void StopPuddleDecay()
    {
        if (puddleShrinkCoro != null)
        {
            StopCoroutine(puddleShrinkCoro);
        }
    }
    
    IEnumerator ShrinkPuddleOverTime()
    {
        while (data.secondsRemainingCharge > 0)
        {
            yield return new WaitForSeconds(data.expiryTimerDecayRate);

            data.secondsRemainingCharge--;

            if (data.secondsRemainingCharge <= 0)
            {
                ExpirePuddle();
            }
        }
    }
    
    void ExpirePuddle()
    {
        AnnounceExpired?.Invoke(this);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if ((puddleToHitLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("Hit " + other.gameObject.name);
            if (other.GetComponentInParent<Health>() != null)
            {
                Health health = other.GetComponentInParent<Health>();
                health.AddStatus(data.associatedStatus);
            }
        }
    }
    
    void HackPlaceOnGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, groundLayer))
        {
            float offset = groundOffset;
            transform.position = hit.point + Vector3.up * offset;
        }
    }
}
