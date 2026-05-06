using UnityEngine;

public class PSIdle : PlayerStateBase
{
   public override void EnterState()
   {
      base.EnterState();
      base.SetStateText("IDLE");
   }
}
