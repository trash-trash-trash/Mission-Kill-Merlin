using System.Collections.Generic;
using UnityEngine;

public class InventoryObjectBrain : MonoBehaviour
{
    public ItemSO itemSO;
    public ItemBase itemBase;
    public Inventory equippedInventory = null;
    
    public bool canEquip = false;
    public bool equipped = false;
    public bool canUse = false;
    
    public InventoryObjectState currentState;

    public GameObject idleState;
    public GameObject equippedState;
    public GameObject useState;
    public GameObject aimState;
    public GameObject throwState;
    public GameObject dropState;
    
    public Dictionary<InventoryObjectState, GameObject> statesDict;
    
    private void Awake()
    {
        statesDict = new Dictionary<InventoryObjectState, GameObject>()
        {
            { InventoryObjectState.Idle, idleState },
            { InventoryObjectState.Equipped, equippedState },
            { InventoryObjectState.Aim, aimState },
            { InventoryObjectState.Use, useState },
            { InventoryObjectState.Throw, throwState },
            { InventoryObjectState.Drop, dropState }
        };
        
        ChangeState(InventoryObjectState.Idle);
    }

    public void ChangeState(InventoryObjectState newState)
    {
        if (statesDict.TryGetValue(currentState, out GameObject currentGO) && currentGO != null)
        {
            currentGO.SetActive(false);
        }

        if (statesDict.TryGetValue(newState, out GameObject newGO) && newGO != null)
        {
            newGO.SetActive(true);
        }

        currentState = newState;
    }

    public bool ReturnCanEquip()
    {
        return canEquip;
    }

    public void Equip(Inventory inventory)
    {
        equippedInventory = inventory;
        ChangeState(InventoryObjectState.Equipped);
    }

    public void Idle()
    {
        ChangeState(InventoryObjectState.Idle);
    }

    public void Drop()
    {
        ChangeState(InventoryObjectState.Drop);
    }

    public void Aim()
    {
        ChangeState(InventoryObjectState.Aim);
    }

    public void Throw()
    {
        ChangeState(InventoryObjectState.Drop);
    }

    public void Use()
    {
        if(canUse)
            ChangeState(InventoryObjectState.Use);
    }

    public InventoryObjectState GetState()
    {
        return currentState;
    }

    public GameObject ReturnSelf()
    {
        return gameObject;
    }

    public ItemSO ReturnItemSO()
    {
        return itemSO;
    }
}

public enum InventoryObjectState
{
    Default,
    Idle,
    Equipped,
    Use,
    Aim,
    Throw,
    Drop
}

