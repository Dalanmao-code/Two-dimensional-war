using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 移动状态
/// </summary>
public class PlayerMoveState : PlayerStateBase
{
    #region 动画器相关
    private int moveBlendHash; //属性
    private float moveBlend; //参数
    private float runThreshold = 0; //奔跑阈值
    private float sprintThreshold = 1; //奔跑阈值
    private float transitionSpeed = 5; //过渡速度
    #endregion

    public override void Init(IStateMachineOwer owner)
    {
        base.Init(owner);
        moveBlendHash = Animator.StringToHash("MoveBlend");
    }
    public override void Enter()
    {
        base.Enter();

        playerModel.PlayStateAnimation("Move");
    }

    public override void Update()
    {
        base.Update();
        if (IsBeControl())
        {

            #region 悬空状态监听
            if (playerController.IsJumping)
            {
                SwitchToHover();
                return;
            }
            #endregion

            #region 待机状态监听
            if (playerController.moveInput.magnitude == 0)
            {
                playerModel.SwitchState(PlayerState.Idle);
                return;
            }
            #endregion

            #region 处理移动速度
            if (playerController.IsSpring)
            {
                moveBlend = Mathf.Lerp(moveBlend, sprintThreshold, transitionSpeed * Time.deltaTime);
            }
            else
            {
                moveBlend = Mathf.Lerp(moveBlend, runThreshold, transitionSpeed * Time.deltaTime);
            }

            playerModel.animator.SetFloat(moveBlendHash, moveBlend);
            #endregion

            #region 处理方向
            //计算移动方向与模型正前方之间的夹角
            float rad = Mathf.Atan2(playerController.localMovement.x, playerController.localMovement.z);
            //旋转到移动方向
            playerModel.transform.Rotate(0, rad * playerController.rotationSpeed * Time.deltaTime, 0);
            #endregion
        }

    }
}
