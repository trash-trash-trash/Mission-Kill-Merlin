using UnityEngine;

public class PSWalking : PlayerStateBase
{
    public override void EnterState()
    {
        base.EnterState();
        base.SetStateText("WALKING");
    }
}
