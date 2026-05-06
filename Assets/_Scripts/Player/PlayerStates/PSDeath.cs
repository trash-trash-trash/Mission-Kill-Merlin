using UnityEngine;

public class PSDeath  : PlayerStateBase
{
    public override void EnterState()
    {
        base.EnterState();
        base.SetStateText("DEAD");
    }
}
