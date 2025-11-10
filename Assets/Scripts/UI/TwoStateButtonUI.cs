using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoStateButtonUI : MonoBehaviour
{
    [Header("是否开启")]
    public bool IsEnter = false;
    [HideInInspector]
    public Button button;
    [HideInInspector]
    public event Action<bool> OnClickEvents;

    [Header("开启时UI")]
    public Sprite OpenSprite;

    [Header("关闭时UI")]
    public Sprite CloseSprite;

    [Header("目标图片")]
    public Image TargetImage; //目标图片

    public void Start()
    {
        button = transform.GetComponentInChildren<Button>();

        //绑定子物体按钮添加事件
        button.onClick.AddListener(() => OnClick());
    }


    /// <summary>
    /// 处理按钮点击事件
    /// </summary>
    public void OnClick()
    {
        if (!IsEnter)
        {
            //Debug.Log("开启");
            if(OnClickEvents != null)
                OnClickEvents?.Invoke(true);

            TargetImage.sprite = OpenSprite;
            IsEnter = true;
        }
        else
        {
            //Debug.Log("关闭");
            if(OnClickEvents!= null)
                OnClickEvents?.Invoke(false);

            TargetImage.sprite = CloseSprite;
            IsEnter = false;
        }
    }
}
