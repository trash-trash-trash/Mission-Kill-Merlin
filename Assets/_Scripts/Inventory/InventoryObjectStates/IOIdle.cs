using UnityEngine;

public class IOIdle : IOStateBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        inventoryObjectBrain.canEquip = true;
        inventoryObjectBrain.equipped = false;
        inventoryObjectBrain.canUse = false;
    }
}
