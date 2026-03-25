using System.Collections.Generic;
using UnityEngine;

public class EnergyBallPool : MonoBehaviour
{
    public GameObject energyBallPrefab;

    public List<GameObject> spawnedAndAvailableEnergyBall = new List<GameObject>();
    public List<GameObject> activeBalls = new List<GameObject>();
    
    public void ShootEnergyBall(Transform owner, Transform shootPoint, Transform targetPoint)
    {
        GameObject newBall = null;
        if (spawnedAndAvailableEnergyBall.Count == 0)
        {
            newBall = Instantiate(energyBallPrefab, shootPoint.position, shootPoint.rotation);
        }
        else
        {
            newBall = spawnedAndAvailableEnergyBall[0];
            spawnedAndAvailableEnergyBall.RemoveAt(0);
        }

        EnergyBall energyBall = newBall.GetComponent<EnergyBall>();
        energyBall.owner = owner;
        energyBall.startPoint.position = shootPoint.position;
        energyBall.startPoint.rotation = shootPoint.rotation;
        energyBall.endPoint.position = targetPoint.position;
        energyBall.endPoint.rotation = targetPoint.rotation;

        activeBalls.Add(newBall);
        energyBall.Init();
        energyBall.gameObject.SetActive(true);

        energyBall.AnnounceExploded += ResetEnergyBall;
    }

    private void ResetEnergyBall(GameObject aObj)
    {
        if (activeBalls.Contains(aObj))
        {
            spawnedAndAvailableEnergyBall.Add(aObj);
            activeBalls.Remove(aObj);
            
            EnergyBall energyBall = aObj.GetComponent<EnergyBall>();
            energyBall.AnnounceExploded -= ResetEnergyBall;
        }
    }
}
