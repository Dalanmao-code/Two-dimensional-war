using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Header("测试延迟")]
    public TextMeshProUGUI pingText;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("连接成功");

        PhotonNetwork.JoinOrCreateRoom("Room", new Photon.Realtime.RoomOptions() { MaxPlayers = 4 }, default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //PhotonNetwork.Instantiate("Player",new Vector3(0,0,0),Quaternion.identity,0);
    }
    // Update is called once per frame
    void Update()
    {
        // 获取当前的Ping值
        int currentPing = PhotonNetwork.GetPing();

        pingText.text = "ms:"+currentPing;

    }
}
