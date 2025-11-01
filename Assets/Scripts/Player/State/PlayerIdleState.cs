using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        playerModel.PlayStateAnimation("Idle");
    }
    // Start is called before the first frame update
    
    // Update is called once per frame
    public override void Update()
    {
        base .Update();

        if (IsBeControl())
        {
            #region ÒÆ¶¯¼àÌý
            if (playerController.moveInput.magnitude != 0)
                playerModel.SwitchState(PlayerState.Move);
            #endregion
        }
    }
}
