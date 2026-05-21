using System.Collections;
using UnityEngine;

public class AttackUseCase : MonoBehaviour
{
    public Inventory attackInventory;

    public InventoryObjectBrain inventoryObject;

    void Start()
    {
        StartCoroutine(HackWait());
    }

    IEnumerator HackWait()
    {
        yield return new WaitForSeconds(1);
        
        attackInventory.Equip(inventoryObject);
    }
}
