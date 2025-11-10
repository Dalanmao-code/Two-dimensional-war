using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
public class ChatManager : MonoBehaviourPun
{
    [HideInInspector]
    public static ChatManager Instance; //创建单例

    public ChatStyleData MyChatStyleList;
    [Tooltip("消息发送处")]
    public Transform SendMessageContent; //消息发送的地方
    [Tooltip("消息预制体")]
    public GameObject MessagePrefabs; //消息预制体
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        #region 测试对话功能
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendInChatStyleList("这是一段测试文本",null);
        }
        #endregion


        

    }

    /// <summary>
    /// 根据样式发送文本
    /// </summary>
    /// <param name="SendMessage">发送消息的文本</param>
    /// <param name="chatStyle">聊天样式文件</param>
    /// <param name="IsMine">是否时在自己运行</param>
    public void SendInChatStyleList(string SendMessage, ChatStyle chatStyle,bool IsMine = true)
    {
        if (IsMine)
            chatStyle = MyChatStyleList.ChatStyleList[0];

        //创建消息预制体
        GameObject MessageText = Instantiate(MessagePrefabs, SendMessageContent);


    }
    
}

[Serializable]
public class ChatStyle
{
    //Id，物体的唯一标识
    public int Id; 
    public string ChinaName;
    public string EnglishName;
    public Vector3 BackGroundScale;
    public Vector2 BackGroundPrivot;
    public Color NameFontColor;
    public Color TextBackgroundColor;
    public Sprite sprite;
    public ChatStyle(string chinaName, string englishName, Vector3 backGroundScale, Sprite sprite, int id, Color nameFontColor, Vector2 backGroundPrivot, Color textBackgroundColor)
    {
        ChinaName = chinaName;
        EnglishName = englishName;
        BackGroundScale = backGroundScale;
        this.sprite = sprite;
        Id = id;
        NameFontColor = nameFontColor;
        BackGroundPrivot = backGroundPrivot;
        TextBackgroundColor = textBackgroundColor;
    }
}
