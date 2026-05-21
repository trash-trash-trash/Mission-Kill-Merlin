using System.Collections;
using UnityEngine;

public class IOUse : IOStateBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(HackWait());
    }

    IEnumerator HackWait()
    {
        yield return new WaitForFixedUpdate();
        ItemUseCase.Instance.UseItem(inventoryObjectBrain, inventoryObjectBrain.equippedInventory);
    }
}