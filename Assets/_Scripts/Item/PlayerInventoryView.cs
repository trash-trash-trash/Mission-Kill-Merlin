using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInventoryView : MonoBehaviour
{
    public InventoryObjectBrain leftEquippedItem;
    public InventoryObjectBrain rightEquippedItem;
    public ItemSO emptyFistSO;

    public Image leftHandImage;
    public Image rightHandImage;
    public TMP_Text leftHandText;
    public TMP_Text rightHandText;

    public Inventory leftHandInventory;
    public Inventory rightHandInventory;

    void Start()
    {
        leftHandInventory.AnnounceInventory += HandleLeftInventory;
        rightHandInventory.AnnounceInventory += HandleRightInventory;
    }


    private void HandleLeftInventory(Inventory inventory, List<InventoryObjectBrain> aRg2)
    {
        if (inventory.equippedItem != null)
        {
            Equip(leftHandInventory, inventory.equippedItem);
        }
        else
        {
            EmptyHand(leftHandInventory);
        }
    }

    private void HandleRightInventory(Inventory inventory, List<InventoryObjectBrain> aRg2)
    {
        if (inventory.equippedItem != null)
        {
            Equip(rightHandInventory, inventory.equippedItem);
        }
        else
        {
            EmptyHand(rightHandInventory);
        }
    }

    void Equip(Inventory inventory, InventoryObjectBrain equippedItem)
    {
        if (inventory == leftHandInventory)
        {
            leftHandImage.sprite = equippedItem.ReturnItemSO().weaponSprite;
            leftHandText.text = equippedItem.ReturnItemSO().name;
        }
        else
        {
            rightHandImage.sprite = equippedItem.ReturnItemSO().weaponSprite;
            rightHandText.text = equippedItem.ReturnItemSO().name;
        }
    }

    void EmptyHand(Inventory inventory)
    {
        if (inventory == leftHandInventory)
        {
            leftHandImage.sprite = emptyFistSO.weaponSprite;
            leftHandText.text = emptyFistSO.name;
            leftEquippedItem = null;
        }
        else
        {
            rightHandImage.sprite = emptyFistSO.weaponSprite;
            rightHandText.text = emptyFistSO.name;
            rightEquippedItem = null;
        }
    }

    void OnDisable()
    {
        leftHandInventory.AnnounceInventory -= HandleLeftInventory;
        rightHandInventory.AnnounceInventory -= HandleRightInventory;
    }
}