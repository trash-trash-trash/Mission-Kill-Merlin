using System;
using UnityEngine;

public interface IInteractable
{
    public bool Interact(IInteract interactee, CharacterActions actionType);

    public GameObject ReturnSelf();

    public bool ReturnCanInteract();
}

public interface IInteract
{
    public void CanInteract(bool canInteract);
}

public interface IHear
{
    public void HeardSound(SoundData sound);

    public void FlipCanHear(bool input);
}

public interface IMemorable
{
    
}

public interface ISee
{
    public bool ReturnCanSee();

    public bool ReturnCanSeePlayer();

    public void ChangeCanSeePlayer(CharacterBase player, bool input);

    public void ChangeCanSee(bool input);
}

public interface ICharacter
{
    public Character ReturnCharacter();

    public CharacterBase ReturnCharacterBase();
}

public interface IInventoryObject
{
    public bool ReturnCanEquip();
    
    public void Equip(Inventory inventory);

    public void Idle();

    public void Drop();

    public void Aim(bool input);
    
    public void Throw();
    
    public void Use();

    public GameObject ReturnSelf();
    
    public InventoryObjectBrain ReturnItemBase();

    public ItemSO ReturnItemSO();
}