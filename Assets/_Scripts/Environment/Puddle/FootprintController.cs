using System.Collections.Generic;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    public PuddleController puddleController;

    [Header("Footstep Settings")]
    public float forwardOffset = 0.5f;
    public float horizontalOffset = 0.2f;

    private bool isLeftStep = true;
    
    public float stepDistance = 1.0f;
    private Vector3 lastStepPos;

    public bool spawningFootsteps = false;

    public Health health;
    
    //combine 
    public HealthStatus mostRecentStatus;

    void Awake()
    {
        health.AnnounceHealthStatus += StartStopFootsteps;
    }

    private void StartStopFootsteps(List<HealthStatus> aHealthStatusList)
    {
        if (aHealthStatusList.Count > 0)
        {
            spawningFootsteps = true;
            mostRecentStatus = aHealthStatusList[0];
        }
        else
        {
            spawningFootsteps = false;
        }
    }

    void Update()
    {
        if (!spawningFootsteps)
            return;
        
        float dist = Vector3.Distance(transform.position, lastStepPos);

        if (dist >= stepDistance)
        {
            SpawnFootstep();
            lastStepPos = transform.position;
        }
    }

    public void SpawnFootstep()
    {
        Vector3 basePos = transform.position;

        // Forward offset
        Vector3 forward = transform.forward * forwardOffset;

        // Left/right offset
        Vector3 side = (isLeftStep ? -transform.right : transform.right) * horizontalOffset;

        Vector3 spawnPos = basePos + forward + side;

        puddleController.SpawnPuddle(CreatePuddleData(mostRecentStatus), spawnPos);

        // Alternate foot
        isLeftStep = !isLeftStep;
        
        Debug.DrawLine(transform.position, spawnPos, Color.blue, 1f);
    }

    private PuddleData CreatePuddleData(HealthStatus newStatus)
    {
        return new PuddleData()
        {
            associatedStatus = newStatus,
            secondsRemainingCharge = puddleController.testChargeCount,
            expiryTimerDecayRate = puddleController.testDecay,
            originalSecondsRemainingCharge = puddleController.testChargeCount
        };
        
    }
}