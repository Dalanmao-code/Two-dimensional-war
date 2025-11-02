using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public enum PlayerState
{
    Idle,Move,Hover,Aiming
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

    #region 约束相关
    public TwoBoneIKConstraint rightHandConstraint; //正常状态下约束
    public MultiAimConstraint rightHandAimConstraint; //瞄准状态下约束
    public MultiAimConstraint BodyAimConstraint; //身躯约束
    #endregion

    #region
    [Tooltip("重力")]
    public float gravity = -15f;
    [Tooltip("默认高度")]
    public float jumpHeight = 1.5f;

    [HideInInspector]
    public float verticalSpeed; //当前垂直速度
    [Tooltip("悬空判定高度")]
    public float fallHeight = 0.2f;
    #endregion

    #region 玩家在地面时前三帧的缓存
    Vector3[] speedCache = new Vector3[3];//动画前三帧的玩家速度
    private int speedCache_index = 0;//缓存保存的位置
    private Vector3 averageDeltaMovement; //平均速度
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (!photonView.IsMine)
        {
            
            return;
        }

        stateMachine = new StateMachine(this);



    }

    void Start()
    {
        if (!photonView.IsMine)
            return;
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
            case PlayerState.Hover:
                stateMachine.EnterState<PlayerHoverState>();
                break;
            case PlayerState.Aiming:
                stateMachine.EnterState<PlayerAimingState>();
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
        if(currentState != PlayerState.Hover)
        {
            UpdateAverageCacheSpeed(animator.velocity);
        }
        else
        {
            playerDeltaMovement = averageDeltaMovement *Time.deltaTime;
        }
        playerDeltaMovement.y = verticalSpeed * Time.deltaTime;
        characterController.Move(playerDeltaMovement);
    }


    /// <summary>
    /// 是否悬空
    /// </summary>
    /// <returns></returns>
    public bool IsHover()
    {
        return !Physics.Raycast(transform.position, Vector3.down, fallHeight);
    }


    /// <summary>
    /// 计算模型前三帧的平均速度
    /// </summary>
    /// <param name="newSpeed">当前速度</param>
    private void UpdateAverageCacheSpeed(Vector3 newSpeed)
    {
        speedCache[speedCache_index++] = newSpeed;
        speedCache_index%= speedCache.Length;
        //计算缓存池中的平均速度
        Vector3 sum = Vector3.zero;
        foreach(Vector3 cache in speedCache)
        {
            sum += cache;
        }
        averageDeltaMovement = sum/speedCache.Length;
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
