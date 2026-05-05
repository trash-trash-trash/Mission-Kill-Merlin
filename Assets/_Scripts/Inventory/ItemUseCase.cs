using UnityEngine;

public class ItemUseCase : MonoBehaviour
{
    public static ItemUseCase Instance { get; private set; }

    public GameObject emptyPotionBottle;

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

        if (itemSO.inventoryObjectType == InventoryObjectType.PotionBottle)
        {
            MagicPotion potion = itemBase.ReturnSelf().GetComponent<MagicPotion>();
            HealthStatus statusType = potion.magicPotionType;
            if(statusType==HealthStatus.Fine)
                Debug.Log("Tried to fill empty bottle!");
            else
            {
                Debug.Log("Drinking from magic bottle! Added "+potion.magicPotionType+" to self!");
                targetInventory.owner.hp.ChangeHP(itemSO.weaponDamage);
            }
            // targetInventory.Unequip(itemBase.ReturnItemBase());
            //
            // Destroy(itemBase.ReturnSelf());
            // GameObject newEmptyPotionBottle = Instantiate(emptyPotionBottle);
            //
            // IInventoryObject item = newEmptyPotionBottle.GetComponent<IInventoryObject>();
            // item.Equip(targetInventory);
            // targetInventory.Equip(newEmptyPotionBottle.GetComponent<ItemBase>());
        }
    }
}