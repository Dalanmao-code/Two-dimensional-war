using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 悬空动画
/// </summary>
public class PlayerHoverState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        playerModel.PlayStateAnimation("Hover");
    }

    public override void Update()
    {
        base.Update();

        #region 检测角色是否在地面上
        if (playerModel.characterController.isGrounded)
            playerModel.SwitchState(PlayerState.Idle);

        #endregion
    }
}
