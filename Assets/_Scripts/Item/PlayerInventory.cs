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

    public LineRenderer line;
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
        if (input.performed)
        {
            leftClickHeld = true;
            leftClickStartedAsAim = false;

            if (shiftHeld && leftHandObject != null)
            {
                Debug.Log("Aiming " + leftHandObject.name);
                leftClickStartedAsAim = true;
                leftHandObject.Aim(true);
                return;
            }

            if (leftHandObject == null)
            {
                IInventoryObject found = use.InteractInSphere();
                if (found != null)
                {
                    Debug.Log("Picked up " + found.ReturnSelf().name);
                    EquipLeftRight(found, true);
                }
            }
            else
            {
                Debug.Log("Using left hand " + leftHandObject.name);
                leftHandObject.Use();
            }
        }

        else
        {
            if (shiftHeld && leftHandObject != null && leftClickStartedAsAim)
            {
                Debug.Log("Throwing " + leftHandObject.name);
                leftHandObject.Throw();
            }
            else if (!shiftHeld && leftHandObject != null)
            {
                Debug.Log("Just holding " + leftHandObject.name);
                leftHandObject.Aim(false);
            }

            leftClickHeld = false;
        }
    }

    private void FlipRightClick(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            rightClickHeld = true;
            rightClickStartedAsAim = false;

            if (shiftHeld && rightHandObject != null)
            {
                Debug.Log("Aiming " + rightHandObject.name);
                rightClickStartedAsAim = true;
                rightHandObject.Aim(true);
                return;
            }

            if (rightHandObject == null)
            {
                IInventoryObject found = use.InteractInSphere();
                if (found != null)
                {
                    Debug.Log("Picked up " + found.ReturnSelf().name);
                    EquipLeftRight(found, false);
                }
            }
            else
            {
                Debug.Log("Using right hand " + rightHandObject.name);
                rightHandObject.Use();
            }
        }
        else
        {
            if (shiftHeld && rightHandObject != null && rightClickStartedAsAim)
            {
                Debug.Log("Throwing " + rightHandObject.name);
                rightHandObject.Throw();
            }
            else if (!shiftHeld && rightHandObject != null)
            {
                Debug.Log("Just holding " + rightHandObject.name);
                rightHandObject.Aim(false);
            }

            rightClickHeld = false;
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
}