using UnityEngine;

public class IOEquipped : IOStateBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        inventoryObjectBrain.equipped = true;
        inventoryObjectBrain.canEquip = false;
        inventoryObjectBrain.canUse = true;
        
        inventoryObjectBrain.gameObject.transform.localPosition = Vector3.zero;
    }
}
