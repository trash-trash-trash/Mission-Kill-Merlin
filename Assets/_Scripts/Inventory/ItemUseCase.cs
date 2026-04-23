using UnityEngine;

public class ItemUseCase : MonoBehaviour
{
    public static ItemUseCase Instance { get; private set; }

    public ItemSO emptyBottleSO;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UseItem(IInventoryObject itemBase, Inventory targetInventory)
    {
        ItemSO itemSO = itemBase.ReturnItemSO();
        Debug.Log($"Using item: "+itemSO.abilityString);

        if (itemSO.inventoryObjectType == InventoryObjectType.HealthPotion)
        {
            targetInventory.owner.hp.ChangeHP(itemSO.weaponDamage);
            itemSO = emptyBottleSO;
            targetInventory.CheckUnarmed();
        }
    }
}