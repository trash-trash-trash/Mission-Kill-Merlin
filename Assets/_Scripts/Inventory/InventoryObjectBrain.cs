using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryObjectBrain : MonoBehaviour, IInventoryObject
{
    public ItemSO itemSO;
    public Inventory equippedInventory = null;
    public Health health;
    public ImpactDamageThreshold impact;
    public Rigidbody rb;
    public Sound sound;

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
    public GameObject brokenState;

    public Dictionary<InventoryObjectState, GameObject> statesDict;

    public virtual void Awake()
    {
        statesDict = new Dictionary<InventoryObjectState, GameObject>()
        {
            { InventoryObjectState.Idle, idleState },
            { InventoryObjectState.Equipped, equippedState },
            { InventoryObjectState.Aim, aimState },
            { InventoryObjectState.Use, useState },
            { InventoryObjectState.Throw, throwState },
            { InventoryObjectState.Drop, dropState },
            { InventoryObjectState.Broken, brokenState }
        };

        ChangeState(InventoryObjectState.Idle);

        health.AnnounceHP += ItemHealthBroke;

        if (impact != null)
            impact.AnnounceHardImpact += ItemImpactBroke;
    }


    public virtual void ItemHealthBroke(int aObj)
    {
        if (aObj <= 0)
            ChangeState(InventoryObjectState.Broken);
    }

    public virtual void ItemImpactBroke(float aObj)
    {
        ChangeState(InventoryObjectState.Broken);
    }

    public virtual void ChangeState(InventoryObjectState newState)
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

    public virtual void HandleEquipped(bool input)
    {
        if (input)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.rotation = equippedInventory.equipPoint.rotation;
            rb.linearVelocity = Vector3.zero;
//            rb.angularVelocity = Vector3.zero;
            equipped = true;
            canEquip = false;
            canUse = true;

            health.canTakeDamage = false;
            impact.checkingForImpact = false;
        }
        else
        {
            transform.parent = null;
            equipped = false;
            canEquip = true;
            canUse = false;

            health.canTakeDamage = true;
            impact.checkingForImpact = true;

            if (equippedInventory != null)
            {
                equippedInventory.Unequip(this);
                equippedInventory = null;
            }
        }
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

    public void Aim(bool input)
    {
        if (input)
        {
            ChangeState(InventoryObjectState.Aim);
        }
        else
        {
            ChangeState(InventoryObjectState.Equipped);
        }
    }

    public void Throw()
    {
        ChangeState(InventoryObjectState.Throw);
    }

    public void Use()
    {
        if (canUse)
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

    public InventoryObjectBrain ReturnItemBase()
    {
        return this;
    }

    public ItemSO ReturnItemSO()
    {
        return itemSO;
    }

    private void OnDisable()
    {
        health.AnnounceHP -= ItemHealthBroke;

        if (impact != null)
            impact.AnnounceHardImpact -= ItemImpactBroke;
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
    Drop,
    Broken
}