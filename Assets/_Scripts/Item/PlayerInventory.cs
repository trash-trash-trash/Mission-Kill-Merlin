using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public PlayerInputHandler playerInputHandler;

    public bool leftClickHeld = false;
    public bool leftClickStartedAsAim = false;
    public bool rightClickHeld = false;
    public bool rightClickStartedAsAim = false;
    public bool shiftHeld = false;
    public bool controlHeld = false;

    public Inventory leftHandInventory;
    public Inventory rightHandInventory;

    public ItemBase leftHandObject = null;
    public ItemBase rightHandObject = null;

    public Use use;

    public bool canInteract = true;

    void Start()
    {
        playerInputHandler.AnnounceLeftClick += FlipLeftClick;
        playerInputHandler.AnnounceRightClick += FlipRightClick;
        playerInputHandler.AnnounceShift += FlipShift;
        playerInputHandler.AnnounceControl += FlipControl;

        leftHandInventory.AnnounceInventory += EquipLeftHand;
        rightHandInventory.AnnounceInventory += EquipRightHand;
    }

    private void EquipRightHand(Inventory aRg1, List<ItemBase> aRg2)
    {
        rightHandObject = aRg1.equippedItem;
    }

    private void EquipLeftHand(Inventory aRg1, List<ItemBase> aRg2)
    {
        leftHandObject = aRg1.equippedItem;
    }

    private void FlipControl(InputAction.CallbackContext input)
    {
        if (input.performed)
            controlHeld = true;
        else
            controlHeld = false;
    }

    private void FlipShift(InputAction.CallbackContext input)
    {
        if (input.performed)
            shiftHeld = true;
        else
            shiftHeld = false;
    }

    private void FlipLeftClick(InputAction.CallbackContext input)
    {
        HandleClick(input, true);
    }

    private void FlipRightClick(InputAction.CallbackContext input)
    {
        HandleClick(input, false);
    }

    private void HandleClick(InputAction.CallbackContext input, bool isLeft)
    {
        ref bool clickHeld = ref (isLeft ? ref leftClickHeld : ref rightClickHeld);
        ref bool startedAsAim = ref (isLeft ? ref leftClickStartedAsAim : ref rightClickStartedAsAim);
        ItemBase item = isLeft ? leftHandObject : rightHandObject;

        if (input.performed)
        {
            clickHeld = true;
            startedAsAim = false;

            if (shiftHeld && item != null)
            {
                Debug.Log($"Aiming {item.name}");
                startedAsAim = true;
                item.Aim(true);
                return;
            }

            if (item == null)
            {
                var found = use.InteractInSphere();
                if (found != null)
                {
                    Debug.Log($"Picked up {found.ReturnSelf().name}");
                    EquipLeftRight(found, isLeft);
                }
            }
            else
            {
                Debug.Log($"Using {(isLeft ? "left" : "right")} hand {item.name}");
                item.Use();
            }
        }
        else
        {
            if (shiftHeld && item != null && startedAsAim)
            {
                Debug.Log($"Throwing {item.name}");
                item.Throw();
            }
            else if (!shiftHeld && item != null)
            {
                Debug.Log($"Just holding {item.name}");
                item.Aim(false);
            }

            clickHeld = false;
        }
    }

    private void EquipLeftRight(IInventoryObject obj, bool left)
    {
        if (left)
        {
            leftHandObject = obj.ReturnItemBase();
            leftHandInventory.Equip(leftHandObject);
            obj.Equip(leftHandInventory);
        }
        else
        {
            rightHandObject = obj.ReturnItemBase();
            rightHandInventory.Equip(rightHandObject);
            obj.Equip(rightHandInventory);
        }
    }

    void OnDisable()
    {
        playerInputHandler.AnnounceLeftClick -= FlipLeftClick;
        playerInputHandler.AnnounceRightClick -= FlipRightClick;
        playerInputHandler.AnnounceShift -= FlipShift;
        playerInputHandler.AnnounceControl -= FlipControl;
        leftHandInventory.AnnounceInventory -= EquipLeftHand;
        rightHandInventory.AnnounceInventory -= EquipRightHand;
    }
}