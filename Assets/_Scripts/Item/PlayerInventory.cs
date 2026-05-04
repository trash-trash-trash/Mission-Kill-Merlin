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
    public bool rightClickHeld = false;
    public bool shiftHeld = false;
    public bool controlHeld = false;

    public Inventory leftHandInventory;
    public Inventory rightHandInventory;

    public Transform leftArm;
    public Transform rightArm;

    public ItemBase leftHandObject = null;
    public ItemBase rightHandObject = null;

    public LineRenderer line;
    public Use use;

    public bool canInteract = true;

    public Transform pointA;
    public Transform pointB;

    void Start()
    {
        playerInputHandler.AnnounceLeftClick += FlipLeftClick;
        playerInputHandler.AnnounceRightClick += FlipRightClick;
        playerInputHandler.AnnounceShift += FlipShift;
        playerInputHandler.AnnounceControl += FlipControl;
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
            leftClickHeld = false;
        }
    }

    private void FlipRightClick(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            rightClickHeld = true;

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
                Debug.Log("Using left hand " + rightHandObject.name);
                rightHandObject.Use();
            }
        }
        else
        {
            rightClickHeld = false;
        }
    }


    private void EquipLeftRight(IInventoryObject obj, bool left)
    {
        if (left)
        {
            leftHandObject = obj.ReturnItemBase();
            leftHandObject.transform.SetParent(leftArm);
            leftHandObject.transform.localPosition = leftArm.transform.position;
            leftHandObject.transform.localRotation = leftArm.transform.rotation;
            pointA = leftArm;
            leftHandInventory.Equip(leftHandObject);
            obj.Equip(leftHandInventory);
        }
        else
        {
            rightHandObject = obj.ReturnItemBase();
            rightHandObject.transform.SetParent(rightArm);
            rightHandObject.transform.localPosition = rightArm.transform.position;
            rightHandObject.transform.localRotation = rightArm.transform.rotation;
            pointA = rightArm;
            rightHandInventory.Equip(rightHandObject);
            obj.Equip(rightHandInventory);
        }
    }
}