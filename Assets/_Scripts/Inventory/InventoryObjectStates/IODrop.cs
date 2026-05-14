using UnityEngine;

public class IODrop  : IOStateBase
{

    public override void OnEnable()
    {
        base.OnEnable();
        // inventoryObjectBrain.rb.isKinematic = false;
        // inventoryObjectBrain.rb.useGravity = true;
        // inventoryObjectBrain.HandleEquipped(false);
    }
}
