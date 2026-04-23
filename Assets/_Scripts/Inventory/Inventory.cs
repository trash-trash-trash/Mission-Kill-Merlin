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

    public event Action<List<ItemBase>> AnnounceInventory;

    public List<ItemBase> inventoryObjects = new List<ItemBase>();

    public virtual void Equip(ItemBase obj)
    {
        inventoryObjects.Add(obj);
        CheckUnarmed();
    }

    public void Use()
    {
        if (unarmed)
            return;
        inventoryObjects[0].Use();
    }

    public void Unequip(ItemBase obj)
    {
        if (inventoryObjects.Contains(obj))
        {
            obj.Drop();
            inventoryObjects.Remove(obj);
        }

        CheckUnarmed();
    }
    
    //throw
    //aim

    public void CheckUnarmed()
    {
        if (inventoryObjects.Count == 0)
            Unarmed = true;
        else
            Unarmed = false;

        AnnounceInventory?.Invoke(inventoryObjects);
    }
}