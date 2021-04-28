using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Text;
using System;

public class Launcher_InRoom : MonoBehaviour
{
    public GameObject inRoomScreen;
    public GameObject awardScreen;
    public GameObject stone;
    public Player player;
    public float timeBack;
    public float timeFall;

    // Start is called before the first frame update
    void Start()
    {
        inRoomScreen = GameObject.Find("Canvas").transform.Find("InRoomScreen").gameObject;
        awardScreen = GameObject.Find("Canvas").transform.Find("AwardScreen").gameObject;
        timeBack = 3.5f;
        timeFall = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdatePlayerList();
        OnBackToTheLobby();
        OnUpdateStoneFall();
    }
    public void OnUpdatePlayerList()
    {
        inRoomScreen.transform.Find("PlayersList").transform.Find("TxtRoomName").GetComponent<Text>().text = "Room Name: " + PhotonNetwork.CurrentRoom.Name.ToString();
        var listplayers = new StringBuilder();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            listplayers.Append(player.NickName + "\n");
        }
        inRoomScreen.transform.Find("PlayersList").transform.Find("TxtPlayersList").GetComponent<Text>().text = listplayers.ToString() + "\n";
    }

    public void OnBackToTheLobby()
    {
        if (awardScreen.activeSelf == true)
        {
            awardScreen.transform.Find("AlertBackTxt").GetComponent<Text>().text = "Back to the lobby in " + Math.Round(timeBack, 0) + " seconds";
            timeBack -= Time.deltaTime;
            if (timeBack <= 0)
            {
                awardScreen.SetActive(false);
                inRoomScreen.SetActive(true);

            }
        }
        else
        {
            timeBack = 3.5f;
            PhotonNetwork.CurrentRoom.IsOpen = true;       //khóa phòng chơi
            PhotonNetwork.CurrentRoom.IsVisible = true;    //ẩn phòng chơi
        }
    }

    public void OnClick_BackToLobbyBtn()  //nút Leave
    {
        awardScreen.SetActive(false);
        inRoomScreen.SetActive(true);
    }

    public void OnUpdateStoneFall()
    {
        if (timeFall <= 0f)
        {
            timeFall = 3.5f;
        }
        else
        {
            timeFall -= Time.deltaTime;
            if (timeFall <= 0)
            {
                Vector3 position1 = new Vector3(UnityEngine.Random.Range(-8f, 10f), 8, 0);
                Vector3 position2 = new Vector3(UnityEngine.Random.Range(-8f, 10f), 9, 0);
                Instantiate(stone, position1, stone.transform.rotation);
                Instantiate(stone, position2, stone.transform.rotation);
            }
        }
        
    }
}
