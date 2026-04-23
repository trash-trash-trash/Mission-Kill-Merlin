using UnityEngine;

public class IOIdle : IOStateBase
{
    public BobTransform bob;
    public RotateTransform rot;
    
    public override void OnEnable()
    {
        base.OnEnable();
        bob =  GetComponentInParent<BobTransform>();
        rot =  GetComponentInParent<RotateTransform>();

        bob.bobbing = true;
        rot.rotating = true;

        inventoryObjectBrain.canEquip = true;
        inventoryObjectBrain.equipped = false;
        inventoryObjectBrain.canUse = false;
    }
    
    void OnDisable()
    {
        bob.bobbing = false;
        rot.rotating = false;
    }
}
