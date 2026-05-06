using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicPotion : InventoryObjectBrain
{
    [Header("Fine is empty")]
    public HealthStatus magicPotionType;
    
    public event Action<HealthStatus> AnnounceBottleType;
    
    public List<GameObject> normalPotionParts;
    public List<GameObject> brokenPotionParts;

    public void SetBottle(HealthStatus status)
    {
        magicPotionType = status;
        AnnounceBottleType?.Invoke(magicPotionType);
    }

    public void BreakBottle()
    {
        foreach (GameObject part in normalPotionParts)
        {
            part.SetActive(false);
        }

        foreach (GameObject potion in brokenPotionParts)
        {
            potion.SetActive(true);
        }

        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public override void ChangeState(InventoryObjectState newState)
    {
        base.ChangeState(newState);
        if (newState == InventoryObjectState.Broken)
        {
            BreakBottle();
        }
    }

}
