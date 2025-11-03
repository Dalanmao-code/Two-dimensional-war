using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Unity.VisualScripting;
using Cinemachine;
using UnityEngine.Animations.Rigging;


/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : MonoBehaviourPun
{
    public PlayerModel currentPlayerModel; //当前操控的角色模型
    public Transform cameralTransform;
    public Camera PlayerCamera;

    [Tooltip("正常视角相机")]
    public CinemachineFreeLook freeLookCamera;
    [Tooltip("瞄准视角相机")]
    public CinemachineFreeLook aimingCamera;
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

    #region 瞄准相关
    [Tooltip("瞄准目标")]
    public Transform AimTarget;
    [Tooltip("射线检测最大距离")]
    public float maxRayDistance = 1000f;
    [Tooltip("射线检测层级")]
    public LayerMask aimLayerMask;
    #endregion

    [Tooltip("转向速度")]
    public float rotationSpeed = 300;

    [HideInInspector]
    public Vector3 localMovement; //本地空间下玩家的移动方向
    [HideInInspector]
    public Vector3 worldMovement; //世界空间下玩家的移动方向

    public bool isMine;
    private void Awake()
    {

        if (!photonView.IsMine)
        {
            AimTarget.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        Application.targetFrameRate = 90;
        input = new MyInputSystem();
        if (IsAndroid)
        {
            movejoystick = GameObject.Find("Variable Joystick").GetComponent<Joystick>();
            Screen.autorotateToLandscapeLeft = true;
        }
    }

    void Start()
    {
        if(!photonView.IsMine)
            cameralTransform.transform.parent.gameObject.SetActive(false);
        else
        {
            if(!IsAndroid)
            Cursor.lockState = CursorLockMode.Locked; //锁定光标
        }
        ExitAim();
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

    /// <summary>
    /// 进入瞄准
    /// </summary>
    public void EnterAim()
    {
        //同步瞄准相机和自由相机角度
        aimingCamera.m_XAxis.Value = freeLookCamera.m_XAxis.Value;
        aimingCamera.m_YAxis.Value = freeLookCamera.m_YAxis.Value;

        //启动瞄准约束
        currentPlayerModel.rightHandAimConstraint.weight = 1;
        currentPlayerModel.BodyAimConstraint.weight = 1;
        currentPlayerModel.rightHandConstraint.weight = 0;
        photonView.RPC("FixedWeight", RpcTarget.Others, true);

        //设置相机的优先级
        freeLookCamera.Priority = 0;
        aimingCamera.Priority = 100;


    }
    public void ExitAim()
    {
        //同步自由相机和瞄准相机角度
        freeLookCamera.m_XAxis.Value = aimingCamera.m_XAxis.Value;
        freeLookCamera.m_YAxis.Value = aimingCamera.m_YAxis.Value;

        //关闭瞄准约束
        currentPlayerModel.rightHandAimConstraint.weight = 0;
        currentPlayerModel.BodyAimConstraint.weight = 0;
        currentPlayerModel.rightHandConstraint.weight = 1;
        photonView.RPC("FixedWeight", RpcTarget.Others, false);

        //设置相机的优先级
        freeLookCamera.Priority = 100;
        aimingCamera.Priority = 0;
    }

    [PunRPC]
    public void FixedWeight(bool Enabled)
    {
        if (!photonView.IsMine)
        {
            if (Enabled)
            {
                currentPlayerModel.rightHandAimConstraint.weight = 1;
                currentPlayerModel.BodyAimConstraint.weight = 1;
                currentPlayerModel.rightHandConstraint.weight = 0;
            }
            else
            {
                currentPlayerModel.rightHandAimConstraint.weight = 0;
                currentPlayerModel.BodyAimConstraint.weight = 0;
                currentPlayerModel.rightHandConstraint.weight = 1;
            }
        }
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
