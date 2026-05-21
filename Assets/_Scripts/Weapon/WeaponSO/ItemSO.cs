using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string name;
    public string abilityString;

    public AttackInfo attackInfo;
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

