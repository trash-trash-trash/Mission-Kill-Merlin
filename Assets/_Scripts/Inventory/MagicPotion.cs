using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicPotion : InventoryObjectBrain
{
    [Header("Fine is empty")]
    public Effects magicPotionType;
    
    public event Action<Effects> AnnounceBottleType;
    
    public List<GameObject> normalPotionParts;
    public List<GameObject> brokenPotionParts;

    public void SetBottle(Effects status)
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
