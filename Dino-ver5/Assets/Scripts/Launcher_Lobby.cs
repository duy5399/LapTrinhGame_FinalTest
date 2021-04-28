using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Text;
using System;

public class Launcher_Lobby : MonoBehaviourPunCallbacks
{
    public GameObject loadingScreen;
    public GameObject connectedScreen;
    public GameObject disconnectedScreen;

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen = GameObject.Find("Canvas").transform.Find("LoadingScreen").gameObject;
        connectedScreen = GameObject.Find("Canvas").transform.Find("ConnectedSceen").gameObject;
        disconnectedScreen = GameObject.Find("Canvas").transform.Find("DisconnectedSceen").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default); //khi kết nối thành công tới Photon Server (Master) thì sẽ kết nối vào Lobby (sảnh chờ) 
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        loadingScreen.SetActive(false);
        disconnectedScreen.SetActive(true); //không kết nỗi bởi lí do gì thì hiện màn hình disconnect
    }

    public override void OnJoinedLobby() //hàm được gọi khi dòng PhotonNetwork.JoinLobby(TypedLobby.Default); thực hiện thành công
    {
        if(loadingScreen.activeSelf)        //activeSelf: đang kích hoạt
            loadingScreen.SetActive(false);
        if (disconnectedScreen.activeSelf)  
            disconnectedScreen.SetActive(false);
        connectedScreen.SetActive(true);    //màn hình chính Lobby sẽ có Create Room và Join Room
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        var listroom = new StringBuilder();
        base.OnRoomListUpdate(roomList);
        foreach (var room in roomList)
        {
            if(room.PlayerCount > 0)
                listroom.Append("Name: " + room.Name + "\t\t" + room.PlayerCount + "/" + room.MaxPlayers + " player");
        }
        connectedScreen.transform.Find("RoomList").transform.Find("Text").GetComponent<Text>().text = listroom.ToString() + "\n";
    }
}
