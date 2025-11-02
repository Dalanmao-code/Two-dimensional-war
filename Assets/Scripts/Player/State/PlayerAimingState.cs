using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 瞄准状态
/// </summary>
public class PlayerAimingState : PlayerStateBase
{
    #region 动画器相关
    private int aimingXHash;
    private int aimingYHash;
    private float aimingX = 0;
    private float aimingY = 0;
    private float transitionSpeed = 5;
    #endregion

    public override void Init(IStateMachineOwer owner)
    {
        base.Init(owner);
        aimingXHash = Animator.StringToHash("AimingX");
        aimingYHash = Animator.StringToHash("AimingY");
    }
    public override void Enter()
    {
        base.Enter();
        playerModel.PlayStateAnimation("Aiming");
        if (IsBeControl())
        {
            UpdateAimingTarget();
            playerController.EnterAim();
        }
    }

    public override void Update()
    {
        base.Update();

        if (IsBeControl())
        {
            //让模型立刻旋转至相机方向
            playerModel.transform.rotation = Quaternion.Euler(0, playerController.cameralTransform.rotation.eulerAngles.y,0);
            UpdateAimingTarget();

            #region 待机监听
            if (!playerController.IsAiming)
            {
                playerModel.SwitchState(PlayerState.Idle);
            }
            #endregion

            #region 处理移动输入
            aimingX = Mathf.Lerp(aimingX, playerController.moveInput.x, transitionSpeed * Time.deltaTime);
            aimingY = Mathf.Lerp(aimingY, playerController.moveInput.y, transitionSpeed * Time.deltaTime);
            playerModel.animator.SetFloat(aimingXHash, aimingX);
            playerModel.animator.SetFloat(aimingYHash, aimingY);
            #endregion

        }
    }

    public override void Exit()
    {
        base.Exit();
        if (IsBeControl())
        {
            playerController.ExitAim();
        }
    }


    /// <summary>
    /// 屏幕中心发射射线确定位置
    /// </summary>
    private void UpdateAimingTarget()
    {
        //发射射线
        Ray ray = playerController.PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //如果射线击中了物体
        if(Physics.Raycast(ray,out hit, playerController.maxRayDistance, playerController.aimLayerMask))
        {
            //更新瞄准目标位置
            playerController.AimTarget.position = hit.point;
        }
        else
        {
            playerController.AimTarget.position = ray.origin+ray.direction*playerController.maxRayDistance;
        }
    }
}
