using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public CharacterBase owner;

    [SerializeField] private bool unarmed = true;

    public bool Unarmed
    {
        get { return unarmed; }
        set { unarmed = value; }
    }

    public event Action<Inventory, List<ItemBase>> AnnounceInventory;

    public List<ItemBase> inventoryObjects = new List<ItemBase>();

    public ItemBase equippedItem;

    public virtual void Equip(ItemBase obj)
    {
        inventoryObjects.Add(obj);
        equippedItem = obj;
        CheckUnarmed();
    }

    public void Use()
    {
        if (unarmed)
            return;
        equippedItem.Use();
    }

    public void Unequip(ItemBase obj)
    {
        if (inventoryObjects.Contains(obj))
        {
            obj.Drop();
            inventoryObjects.Remove(obj);
            equippedItem = null;
        }

        CheckUnarmed();
    }
    
    //throw
    //aim

    public void CheckUnarmed()
    {
        if (equippedItem == null)
            Unarmed = true;
        else
            Unarmed = false;

        AnnounceInventory?.Invoke(this, inventoryObjects);
    }
}