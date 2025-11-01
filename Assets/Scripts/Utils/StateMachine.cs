using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStateMachineOwer { } //状态宿主主机
public class StateMachine{

    private StateBase currentState; //当前状态
    private IStateMachineOwer owner; // 状态宿主
    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();//状态数组

    public StateMachine(IStateMachineOwer owner)
    {
        this.owner = owner;
    }
    /// <summary>
    /// 进入动画状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void EnterState<T>() where T : StateBase,new()
    {
        if (currentState!=null&&currentState.GetType() == typeof(T))
            return;

        if (currentState != null)
            currentState.Exit();
        currentState = LoadState<T>();
        currentState.Enter();
    }

    /// <summary>
    /// 尝试从字典中取出状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private StateBase LoadState<T>() where T : StateBase, new()
    {
        Type stateType =  typeof(T);
        if(!stateDic.TryGetValue(stateType,out StateBase state))
        {
            state = new T();
            state.Init(owner);
            stateDic.Add(stateType,state);
        }
        return state;
    }

    /// <summary>
    /// 停止状态机
    /// </summary>
    public void Stop()
    {
        if(currentState != null)
            currentState.Exit();
        foreach(var state in stateDic.Values)
        {
            state.Destroy();
        }
        stateDic.Clear();
    }
}
