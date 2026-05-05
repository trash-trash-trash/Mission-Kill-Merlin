using UnityEngine;

public class IOThrow  : IOStateBase
{
    public override void OnEnable()
    {
        base.OnEnable();

        inventoryObjectBrain.rb.isKinematic = false;
        inventoryObjectBrain.rb.useGravity = true;
        inventoryObjectBrain.lineArc.ThrowRigidBodyAlongArc(inventoryObjectBrain.rb);
        
        inventoryObjectBrain.HandleEquipped(false);
    }
}
