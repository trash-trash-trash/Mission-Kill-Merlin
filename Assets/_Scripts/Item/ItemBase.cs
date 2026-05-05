using UnityEngine;

public class ItemBase : MonoBehaviour, IInventoryObject
{
    public InventoryObjectBrain brain;
    
    public bool ReturnCanEquip()
    {
        return brain.canEquip;
    }

    public void Equip(Inventory inventory)
    {
        brain.Equip(inventory);
    }

    public void Idle()
    {
        brain.Idle();
    }

    public void Drop()
    {
        brain.Drop();
    }

    public void Aim(bool input)
    {
        brain.Aim(input);
    }

    public void Throw()
    {
        brain.Throw();
    }

    public void Use()
    {
        brain.Use();
    }

    public GameObject ReturnSelf()
    {
        return gameObject;
    }

    public ItemBase ReturnItemBase()
    {
        return brain.itemBase;
    }

    public ItemSO ReturnItemSO()
    {
        return brain.itemSO;
    }
}