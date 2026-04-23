using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    /*public PlayerInputHandler playerInputHandler;

    public Shove shove;
    
    public bool leftClickHeld = false;
    public bool rightClickHeld = false;
    public bool shiftHeld = false;
    public bool controlHeld = false;

    public Inventory leftHandInventory;
    public Inventory rightHandInventory;

    public Transform leftArm;
    public Transform rightArm;

    public GameObject leftHandObject = null;
    public GameObject rightHandObject = null;

    public LineRenderer line;
    public Use use;

    public bool canInteract = true;
    
    public Transform pointA;  
    public Transform pointB;  
    public LayerMask hitLayers;
    public Transform shootPoint;

    public StraightLineRenderer leftAimer;
    public StraightLineRenderer rightAimer;

    public bool aiming = false;
    
    void Start()
    {
        playerInputHandler.AnnounceLeftClick += FlipLeftClick;
        playerInputHandler.AnnounceRightClick += FlipRightClick;
        playerInputHandler.AnnounceShift += FlipShift;
        playerInputHandler.AnnounceControl += FlipControl;

        playerInputHandler.AnnounceQ += UseLeft;
        playerInputHandler.AnnounceR += UseRight;
    }


    private void UseLeft(InputAction.CallbackContext context)
    {
        if(leftHandInventory.Unarmed)
            shove.TryShove(leftArm);

        else
        {
            leftHandInventory.Use();
        }
    }
    
    private void UseRight(InputAction.CallbackContext context)
    {
        if(rightHandInventory.Unarmed)
            shove.TryShove(rightArm);
        
        else
        {
            rightHandInventory.Use();
        }
    }

    private void FlipControl(InputAction.CallbackContext input)
    {
        if(input.performed)
            controlHeld = true;
        else
            controlHeld = false;
    }

    private void FlipShift(InputAction.CallbackContext input)
    {
        if(input.performed)
            shiftHeld = true;
        else
            shiftHeld = false;
    }

    private void FlipRightClick(InputAction.CallbackContext input)
    {
        if(input.performed)
        {
            rightClickHeld = true;
            
            if(rightHandObject == null)
            {
                IInventoryObject found = use.InteractInSphere();
                if (found != null)
                {
                    Debug.Log("Interacting with: " + found.ReturnSelf().name);
                    EquipLeftRight(found, false);
                }
            }
            else
            {
                rightAimer.aiming = true;
            }
        }
        else
        {
            rightClickHeld = false;
            rightAimer.aiming = false;
        }
    }

    private void FlipLeftClick(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            leftClickHeld = true;
            if(leftHandObject == null)
            {
                IInventoryObject found = use.InteractInSphere();
                if (found != null)
                {
                    Debug.Log("Interacting with: " + found.ReturnSelf().name);
                    EquipLeftRight(found, true);
                }
            }
            else
            {
                leftAimer.aiming = true;
            }
        }
        
        else
        {
            leftClickHeld = false;
            leftAimer.aiming = false;
        }
    }

    private void EquipLeftRight(ItemBase obj, bool left)
    {
        if (left)
        {
            leftHandObject = obj.ReturnSelf();
            leftHandObject.transform.SetParent(leftArm);
            leftHandObject.transform.localPosition = leftArm.transform.position;
            leftHandObject.transform.localRotation = leftArm.transform.rotation;
            pointA = leftArm;
            leftHandInventory.Equip(obj);
            obj.Equip(leftHandInventory);
        }
        else
        {
            rightHandObject = obj.ReturnSelf();
            rightHandObject.transform.SetParent(rightArm);
            rightHandObject.transform.localPosition = rightArm.transform.position;
            rightHandObject.transform.localRotation = rightArm.transform.rotation;
            pointA = rightArm;
            rightHandInventory.Equip(obj);
            obj.Equip(rightHandInventory);
        }

    }
    
    void ShowHitLine(Vector3 start, Vector3 end, bool input)
    {
        if (input)
        {
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.startColor = Color.red;
            line.endColor = Color.red;
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }

    void Update()
    {
        if (aiming && line)
        {
            ShowHitLine(pointA.position, pointB.position,true);
        }
        else if (line && line.enabled)
            ShowHitLine(pointA.position, pointB.position, false);
    }*/
}
