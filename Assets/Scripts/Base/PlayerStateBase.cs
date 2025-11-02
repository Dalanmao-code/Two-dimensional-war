using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : StateBase
{
    protected PlayerController playerController;
    protected PlayerModel playerModel; //当前状态的角色模型

    public override void Init(IStateMachineOwer owner)
    {
        playerController = ((PlayerModel)owner).playerController;
        playerModel = (PlayerModel)owner;
    }

    public override void Destroy()
    {
    }

    public override void Enter()
    {
        MonoManager.INSTANCE.AddUpdateAction(Update);
    }

    public override void Exit()
    {
        MonoManager.INSTANCE.RemoveUpdateAction(Update);
    }

    public override void Update()
    {
        #region 重力计算
        if (!playerModel.characterController.isGrounded){ //只有不在地面
            playerModel.verticalSpeed += playerModel.gravity * Time.deltaTime; //施加重力

        }
        else
            playerModel.verticalSpeed = playerModel.gravity * Time.deltaTime;
        #endregion
    }


    /// <summary>
    /// 当前模型是否被玩家控制
    /// </summary>
    /// <returns></returns>
    public bool IsBeControl()
    {
        return playerModel == playerController.currentPlayerModel;
    }

    /// <summary>
    /// 切换方法方法
    /// </summary>
    public void SwitchToHover()
    {
        //计算跳跃速度
        playerModel.verticalSpeed = Mathf.Sqrt(-2 * playerModel.gravity * playerModel.jumpHeight);
        //切换到悬空状态
        playerModel.SwitchState(PlayerState.Hover);
    }
}
