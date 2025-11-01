using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PlayerState
{
    Idle,Move
}

/// <summary>
/// 角色模型
/// </summary>
public class PlayerModel : MonoBehaviourPun,IStateMachineOwer
{
    [HideInInspector]
    public Animator animator;
    public CharacterController characterController;
    public PlayerController playerController; //角色操控

    private StateMachine stateMachine; //动画状态机
    public PlayerState currentState; //当前状态

    #region
    [Tooltip("重力")]
    public float gravity = -15f;
    [Tooltip("默认高度")]
    public float jumpHeight = 1.5f;

    [HideInInspector]
    public float verticalSpeed; //当前垂直速度
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        stateMachine = new StateMachine(this);
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        if(photonView.IsMine)
        SwitchState(PlayerState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    public void SwitchState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                stateMachine.EnterState<PlayerIdleState>();
                break;
            case PlayerState.Move:
                stateMachine.EnterState<PlayerMoveState>();
                break;
        }
        currentState = state;
    }

    
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    /// <param name="transition">过渡时间</param>
    /// <param name="layer">动画层</param>
    public void PlayStateAnimation(string animationName,float transition = 0.25f,int layer = 0)
    {
        animator.CrossFadeInFixedTime(animationName,transition,layer);

        // 告诉所有其他客户端也播放跳跃动画
        photonView.RPC("RPC_PlayJumpAnimation", RpcTarget.Others, animationName, transition, layer);
    }

    public void OnAnimatorMove()
    {
        if (!photonView.IsMine)
            return;
        Vector3 playerDeltaMovement = animator.deltaPosition; //获取动画控制器当前位置
        playerDeltaMovement.y = verticalSpeed *Time.deltaTime;
        characterController.Move(playerDeltaMovement);
    }


    [PunRPC]
    void RPC_PlayJumpAnimation(string animationName, float transition, int layer)
    {
        if (!photonView.IsMine) // 确保不会重复执行
        {
            animator.CrossFadeInFixedTime(animationName, transition, layer);
        }
    }
}
