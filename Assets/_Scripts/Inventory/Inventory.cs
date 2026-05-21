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

    public event Action<Inventory, List<InventoryObjectBrain>> AnnounceInventory;

    public List<InventoryObjectBrain> inventoryObjects = new List<InventoryObjectBrain>();

    public InventoryObjectBrain equippedItem;

    public Transform equipPoint;

    public Transform forwardReference;

    public LineArc lineArc;
    
    public virtual void Equip(InventoryObjectBrain obj)
    {
        inventoryObjects.Add(obj);
        equippedItem = obj;

        Transform objTransform = obj.ReturnSelf().transform;
        objTransform.SetParent(equipPoint);
        objTransform.localPosition = equipPoint.localPosition;
        objTransform.localRotation = equipPoint.localRotation;
        
        lineArc.forwardReference = forwardReference;
        lineArc.startPoint = equipPoint;
        
        obj.Equip(this);
        obj.HandleEquipped(true);
        
        CheckUnarmed();
    }

    public void Use()
    {
        if (unarmed)
            return;
        equippedItem.Use();
    }

    public void Unequip(InventoryObjectBrain obj)
    {
        if (inventoryObjects.Contains(obj))
        {
            obj.Drop();
            inventoryObjects.Remove(obj);
            equippedItem = null;
        }
        
        lineArc.forwardReference = null;
        lineArc.startPoint = null;
        
        obj.HandleEquipped(false);
        
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