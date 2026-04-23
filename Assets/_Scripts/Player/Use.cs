using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Use : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask interactableLayers;

    public Transform originPoint;

    public event Action<IInventoryObject> AnnounceItemFound;

    public void TryUse()
    {
        InteractInSphere();
    }

    public IInventoryObject InteractInSphere()
    {
        Debug.Log("Looking");
        bool foundSomething = false;
        Collider[] hits = Physics.OverlapSphere(originPoint.position, radius, interactableLayers);

        foreach (Collider col in hits)
        {
            IInventoryObject item = col.GetComponent<IInventoryObject>();
            if (item == null)
            {
                // Try on parent if not found on current collider
                item = col.GetComponentInParent<IInventoryObject>();
            }

            if (item != null && item.ReturnCanEquip())
            {
                //interactable.Interact(this);

                Debug.Log("Found "+item.ReturnSelf().name);
                AnnounceItemFound?.Invoke(item);
                return item;
            }

        }
        return null;
    }
}