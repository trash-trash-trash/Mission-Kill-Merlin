using UnityEngine;

public class NPCInventory : MonoBehaviour
{
    //one weapon at a time for now

    public WeaponSO equippedWeapon;
    
    public void EquipWeapon(WeaponSO weaponToEquip)
    {
        equippedWeapon = weaponToEquip;
    }

    public void DropWeapon()
    {
        
    }
}
