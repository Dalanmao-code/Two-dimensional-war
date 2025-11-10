using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneClickUi : MonoBehaviour
{
    [HideInInspector]
    public Button button;
    [HideInInspector]
    public event Action OnClickEvents;

    // Start is called before the first frame update
    void Start()
    {
        button = transform.GetComponentInChildren<Button>();

        //绑定子物体按钮添加事件
        button.onClick.AddListener(() => OnClick());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        OnClickEvents?.Invoke();
    }
}
