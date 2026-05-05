using System.Collections.Generic;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
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

    public Transform transformToSpawnAt;

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
        
        float dist = Vector3.Distance(transformToSpawnAt.position, lastStepPos);

        if (dist >= stepDistance)
        {
            SpawnFootstep();
            lastStepPos = transformToSpawnAt.position;
        }
    }

    public void SpawnFootstep()
    {
        Vector3 basePos = transformToSpawnAt.position;

        // Forward offset
        Vector3 forward = transformToSpawnAt.forward * forwardOffset;

        // Left/right offset
        Vector3 side = (isLeftStep ? -transformToSpawnAt.right : transformToSpawnAt.right) * horizontalOffset;

        Vector3 spawnPos = basePos + forward + side;

        PuddleController.Instance.SpawnPuddle(CreatePuddleData(mostRecentStatus), spawnPos);

        // Alternate foot
        isLeftStep = !isLeftStep;
        
        Debug.DrawLine(transformToSpawnAt.position, spawnPos, Color.blue, 1f);
    }

    private PuddleData CreatePuddleData(HealthStatus newStatus)
    {
        PuddleController puddleController = PuddleController.Instance;
        return new PuddleData()
        {
            associatedStatus = newStatus,
            secondsRemainingCharge = puddleController.testChargeCount,
            expiryTimerDecayRate = puddleController.testDecay,
            originalSecondsRemainingCharge = puddleController.testChargeCount
        };
        
    }
}