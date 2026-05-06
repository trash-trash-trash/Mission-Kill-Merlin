using UnityEngine;

public class PSTakeDamage : PlayerStateBase
{
    public override void EnterState()
    {
        base.EnterState();
        base.SetStateText("TAKE DAMAGE");
    }
}
