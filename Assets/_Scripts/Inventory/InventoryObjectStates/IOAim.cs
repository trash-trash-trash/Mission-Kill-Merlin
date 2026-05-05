using UnityEngine;

public class IOAim : IOStateBase
{

    public override void OnEnable()
    {
        base.OnEnable();
        inventoryObjectBrain.lineArc.drawingArc = true;
    }

    public void OnDisable()
    {
        inventoryObjectBrain.lineArc.drawingArc = false;
    }
}