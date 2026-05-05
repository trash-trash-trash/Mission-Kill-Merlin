using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string name;
    public string abilityString;
    
    public float range;
    public float fireRate;

    public int weaponDamage;
    
    public float weaponForce;
    public float weaponForceTorque;
    public Sprite weaponSprite;

    public InventoryObjectType  inventoryObjectType;
    
    public SoundData soundData;
}

[Serializable]
public enum InventoryObjectType
{
    MeleeWeapon,
    RangeWeapon,
    PotionBottle
}

