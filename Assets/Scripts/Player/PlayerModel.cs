using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,Move
}

/// <summary>
/// 角色模型
/// </summary>
public class PlayerModel : MonoBehaviour,IStateMachineOwer
{
    [HideInInspector]
    public Animator animator;
    public PlayerController playerController; //角色操控

    private StateMachine stateMachine; //动画状态机
    public PlayerState currentState; //当前状态
    // Start is called before the first frame update
    private void Awake()
    {
        stateMachine = new StateMachine(this);
        animator = GetComponent<Animator>();
    }

    void Start()
    {
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
    }
}
