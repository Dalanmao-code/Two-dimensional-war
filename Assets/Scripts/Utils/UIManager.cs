using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管理Ui部分
/// </summary>
public class UIManager : MonoBehaviour
{
    #region 单例模式
    //一个简单的单例
    public static UIManager Instance;
    #endregion
    // Start is called before the first frame update

    #region 储存聊天框UI
    public bool IsEnabledChat= true; //是否启用聊天框
    public GameObject ChatAllUI;  //储存聊天面板
    private bool IsOpenChat = false;
    #endregion

    private void OnEnable()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControlUI();
    }

    #region 按键对应UI
    public void PlayerControlUI()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenOrCloseChatUI();
        }
    }
    #endregion

    #region 开启聊天框UI
    public void OpenOrCloseChatUI()
    {
        if (IsOpenChat)
        {
            if (GameManager.MinePlayer != null)
            {
                GameManager.MinePlayer.UnLockedPlayer();
            }


            ChatAllUI.GetComponent<Ricimi.Popup>().Close();
            //PlayerMoveController.IsLocted = true;
            IsOpenChat = false;
        }
        else
        {

            if (GameManager.MinePlayer != null)
            {
                GameManager.MinePlayer.LockedPlayer();
            }

            ChatAllUI.SetActive(true);
            ChatAllUI.GetComponent<Ricimi.Popup>().Open();
            IsOpenChat = true;
        }
    }
    #endregion
}
