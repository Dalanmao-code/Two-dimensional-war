using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Unity.VisualScripting;


/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : MonoBehaviourPun
{
    public PlayerModel currentPlayerModel; //当前操控的角色模型
    public Transform cameralTransform;
    [Tooltip("安卓相关")]
    public Joystick movejoystick;

    private static bool IsAndroid = false;


    #region 玩家输入相关
    private MyInputSystem input; //输入系统
    [HideInInspector]
    public Vector2 moveInput; //移动输入
    [HideInInspector]
    public bool IsSpring; //冲刺输入
    [HideInInspector]
    public bool IsAiming; //瞄准输入
    [HideInInspector]
    public bool IsJumping; //跳跃输入
    #endregion

    [Tooltip("转向速度")]
    public float rotationSpeed = 300;

    [HideInInspector]
    public Vector3 localMovement; //本地空间下玩家的移动方向
    [HideInInspector]
    public Vector3 worldMovement; //世界空间下玩家的移动方向

    private void Awake()
    {
        if (!photonView.IsMine)
            return;
        Application.targetFrameRate = 90;
        input = new MyInputSystem();
        if(IsAndroid)
        movejoystick = GameObject.Find("Variable Joystick").GetComponent<Joystick>();
    }

    void Start()
    {
        if(!photonView.IsMine)
            cameralTransform.transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;
        #region 更新玩家输入
        if (!IsAndroid)
            moveInput = input.Player.Move.ReadValue<Vector2>().normalized;
        else
            moveInput = new Vector2(movejoystick.Horizontal, movejoystick.Vertical);

        IsSpring = input.Player.IsSprint.IsPressed();
        IsAiming = input.Player.IsAiming.IsPressed();
        IsJumping = input.Player.IsJumping.IsPressed();
        #endregion

        #region 计算玩家移动方向
        //获取相机的方向向量
        Vector3 cameraForwardProjection =new Vector3(cameralTransform.forward.x,0,cameralTransform.forward.z).normalized;
        //计算世界空间下的方向向量
        worldMovement = cameraForwardProjection * moveInput.y + cameralTransform.right * moveInput.x;
        //将世界转换为本地
        localMovement = currentPlayerModel.transform.InverseTransformVector(worldMovement);
        #endregion
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
