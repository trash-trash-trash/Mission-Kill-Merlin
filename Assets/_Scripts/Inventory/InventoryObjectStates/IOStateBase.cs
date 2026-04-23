using UnityEngine;

public class IOStateBase : MonoBehaviour
{
    public InventoryObjectBrain inventoryObjectBrain;

    public virtual void OnEnable()
    {
        inventoryObjectBrain = GetComponentInParent<InventoryObjectBrain>();
    }
}
