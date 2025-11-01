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

    }


    /// <summary>
    /// 当前模型是否被玩家控制
    /// </summary>
    /// <returns></returns>
    public bool IsBeControl()
    {
        return playerModel == playerController.currentPlayerModel;
    }
}
