using UnityEngine;

public class IOEquipped : IOStateBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        inventoryObjectBrain.HandleEquipped(true);
    }
}
