using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteract>() != null)
        {
            IInteract interact = other.GetComponent<IInteract>();
            interact.CanInteract(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteract>() != null)
        {
            IInteract interact = other.GetComponent<IInteract>();
            interact.CanInteract(false);
        }
    }
}