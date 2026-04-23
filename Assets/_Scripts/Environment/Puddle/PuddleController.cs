using System.Collections.Generic;
using UnityEngine;

public class PuddleController : MonoBehaviour
{
    public GameObject puddlePrefab;
    
    public List<GameObject> spawnedPuddles = new List<GameObject>();
    public List<GameObject> availablePuddles = new List<GameObject>();
    public List<GameObject> unavailablePuddles = new List<GameObject>();

    public HealthStatus testStatus;
    public int testChargeCount;
    public float testDecay;

    public void TestSpawn()
    {
        PuddleData newPuddleData = new PuddleData()
        {
            associatedStatus = testStatus,
            secondsRemainingCharge = testChargeCount,
            expiryTimerDecayRate = testDecay,
            originalSecondsRemainingCharge = testChargeCount
        };
        SpawnPuddle(newPuddleData, transform.position);
    }

    public void SpawnPuddle(PuddleData newPuddleData, Vector3 spawnPosition)
    {
        if (availablePuddles.Count == 0)
        {
            GameObject puddle = Instantiate(puddlePrefab, spawnPosition, Quaternion.identity, transform);
            spawnedPuddles.Add(puddle);
            unavailablePuddles.Add(puddle);
            
            Puddle pdle = puddle.GetComponent<Puddle>();
            pdle.SetPuddleType(newPuddleData);
            pdle.AnnounceExpired += AddToPool;
        }
        else
        {
            GameObject puddle = availablePuddles[0];
            puddle.transform.position = spawnPosition;
            availablePuddles.RemoveAt(0);
            unavailablePuddles.Add(puddle);
            
            Puddle pdle = puddle.GetComponent<Puddle>();
            puddle.SetActive(true);
            pdle.SetPuddleType(newPuddleData);
        }
    }

    private void AddToPool(Puddle aPuddle)
    {
        GameObject puddleObj = aPuddle.gameObject;
        if(unavailablePuddles.Contains(puddleObj))
        {
            aPuddle.StopPuddleDecay();
            unavailablePuddles.Remove(puddleObj);
            availablePuddles.Add(puddleObj);
            puddleObj.SetActive(false);
        }
    }
}
