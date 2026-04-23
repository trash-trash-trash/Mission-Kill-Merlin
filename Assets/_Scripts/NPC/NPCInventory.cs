using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : Inventory
{
    //one weapon at a time for now
  
    public List<InventoryObjectBrain> inventoryObjectsToEquip = new List<InventoryObjectBrain>();

    void OnEnable()
    {
        foreach (InventoryObjectBrain inventoryObject in inventoryObjectsToEquip)
        {
         //   Equip(inventoryObject);
        }
    }
}
