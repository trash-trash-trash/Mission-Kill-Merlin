using UnityEngine;

public class IOAim : IOStateBase
{

    public override void OnEnable()
    {
        base.OnEnable();
        inventoryObjectBrain.equippedInventory.lineArc.drawingArc = true;
    }

    public void OnDisable()
    {
        inventoryObjectBrain.equippedInventory.lineArc.drawingArc = false;
    }
}