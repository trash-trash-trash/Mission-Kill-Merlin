using System.Collections.Generic;
using UnityEngine;

public enum WeaponStates
{
    IdlePutDown,
    IdleEquipped,
    Windup,
    Attack01,
    Cooldown
}

public class WeaponBrain : InventoryObjectBrain
{
    public WeaponStates currentWeaponState;
    private GameObject prevWeaponState;
    
    public GameObject idlePutdownState;
    public GameObject idleEquippedState;
    public GameObject windupState;
    public GameObject attack01State;
    public GameObject cooldownState;

    public Dictionary<WeaponStates, GameObject> statesDict = new Dictionary<WeaponStates, GameObject>();

    void Awake()
    {
        base.Awake();
        
        statesDict.Add(WeaponStates.IdlePutDown, idlePutdownState);
        statesDict.Add(WeaponStates.IdleEquipped,  idleEquippedState);
        statesDict.Add(WeaponStates.Windup,  windupState);
        statesDict.Add(WeaponStates.Attack01, attack01State);
        statesDict.Add(WeaponStates.Cooldown,  cooldownState);
        
        
        if(equippedInventory!=null)
            ChangeWeaponState(WeaponStates.IdleEquipped);
        else
            ChangeWeaponState(WeaponStates.IdlePutDown);
    }

    public void ChangeWeaponState(WeaponStates newState)
    {
        if (statesDict.TryGetValue(newState, out GameObject stateObj))
        {
            statesDict[newState] = stateObj;

            if (prevWeaponState != null)
                prevWeaponState.SetActive(false);
            
            currentWeaponState = newState;
            stateObj.SetActive(true);
            prevWeaponState = stateObj;
        }
    }
}
