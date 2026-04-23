using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class HandUI
{
    public Image weaponImage;
    public Image abilityImage;

    public TMP_Text weaponText;
    public TMP_Text abilityText;
}
public class PlayerInventoryView : MonoBehaviour
{
    public ItemSO emptyFistSO;

    public HandUI leftHandUI;
    public HandUI rightHandUI;

    public Inventory leftHandInventory;
    public Inventory rightHandInventory;

    void Start()
    {
        // leftHandInventory.AnnounceInventory += (items) => HandleInventory(items, leftHandUI);
        // rightHandInventory.AnnounceInventory += (items) => HandleInventory(items, rightHandUI);
    }
    
    private void HandleInventory(List<InventoryObjectBrain> items, HandUI ui)
    {
        ItemSO itemSO;

        if (items == null || items.Count == 0)
        {
            itemSO = emptyFistSO;
        }
        else
        {
            itemSO = items[0].ReturnItemSO();
        }

        ui.weaponImage.sprite = itemSO.weaponSprite;
        ui.abilityImage.sprite = itemSO.weaponSprite;

        ui.weaponText.text = itemSO.name;
        ui.abilityText.text = itemSO.abilityString;
    }

    void OnDestroy()
    {
        
    }
    
}