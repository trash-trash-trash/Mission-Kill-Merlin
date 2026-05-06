using UnityEngine;

public class PSDash  : PlayerStateBase
{
    public override void EnterState()
    {
        base.EnterState();
        base.SetStateText("DASHING");
        playerBrain.playerMove.AnnounceDash += EndDash;
    }

    private void EndDash(bool aObj)
    {
        if(!aObj)
        {
            if(playerBrain.playerMove.inputDirection == Vector2.zero)
                playerBrain.ChangeState(PlayerState.Idle);
            else
                playerBrain.ChangeState(PlayerState.Dash);
        }
    }

    void OnDisable()
    {
        playerBrain.playerMove.AnnounceDash -= EndDash;
    }
}
