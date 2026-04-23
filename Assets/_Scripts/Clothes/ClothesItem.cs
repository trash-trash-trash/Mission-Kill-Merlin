using UnityEngine;

public class ClothesItem : MonoBehaviour, IInteractable
{
    public Clothes clothes;
    private bool canInteract = true;

    public bool Interact(IInteract interactee, CharacterActions actionType)
    {
        if (!canInteract)
            return false;
        
        return true;
    }

    public GameObject ReturnSelf()
    {
        return gameObject;
    }

    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }
    public bool ReturnCanInteract()
    {
        return CanInteract;
    }
}
