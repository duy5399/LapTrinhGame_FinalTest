using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIhandler : MonoBehaviourPunCallbacks
{
    public GameObject[] characters;
    //public GameObject mortPrefab;
    //public GameObject douxPrefab;

    public GameObject setNameScreen;
    public GameObject inRoomScreen;
    public InputField nameTxt;

    public GameObject alertQuit;
    public GameObject alertRoom;
    public InputField createRoomTxt;
    public InputField joinRoomTxt;

    public Button connectBtn;
    public Button createRoomBtn;
    public Button joinRoomBtn;
    public Button startMatchBtn;
    public Button leaveRoomBtn;
    public Button backToLobbyBtn;

    // Start is called before the first frame update
    void Start()
    {
        alertQuit = GameObject.Find("Canvas").transform.Find("AlertQuit").gameObject;
        //setNameScreen = GameObject.Find("Canvas").transform.Find("SetNameScreen").gameObject;
    }

    //Login
    public void OnChange_NamePlayer()    //hàm gọi khi giá trị ở ô Text Box nhập tên thay đổi
    {
        if (nameTxt.text.Length > 2)    //kiểm tra tên nhập vào lớn hơn 2 kí tự
        {
            connectBtn.interactable = true; //button Connect sẽ enable //interactable: tương tác
        }
        else
            connectBtn.interactable = false;
    }

    public void OnClick_SetName()       
    {
        PhotonNetwork.NickName = nameTxt.text;
    }

    public void OnClick_ConnectBtn()
    {
        PhotonNetwork.ConnectUsingSettings(); //khi nhấn Connect_button sẽ kết nối tới Photon Server theo App ID tại PhotonServerSettings
        PhotonNetwork.LoadLevel(1);
    }

    public void SpawnPlayer()  //tạo - sản sinh nhân vật khi vào game 
    {
        Vector3 position = new Vector3(Random.Range(-8f, 10f), 7, 0);
        switch (PlayerPrefs.GetInt("selectedCharacter"))
        {
            case 0:
                PhotonNetwork.Instantiate(characters[0].name, position, characters[0].transform.rotation);    //Instantiate: khởi tạo
                break;
            case 1:
                PhotonNetwork.Instantiate(characters[1].name, position, characters[1].transform.rotation);    //Instantiate: khởi tạo
                break;
            case 2:
                PhotonNetwork.Instantiate(characters[2].name, position, characters[2].transform.rotation);    //Instantiate: khởi tạo
                break;
            case 3:
                PhotonNetwork.Instantiate(characters[3].name, position, characters[3].transform.rotation);    //Instantiate: khởi tạo
                break;
        }
        //PhotonNetwork.Instantiate(douxPrefab.name, position, douxPrefab.transform.rotation);    //Instantiate: khởi tạo
    }

    public void OnClick_ContinueBtn()   //nút Continue (Set name nhân vật)
    {
        setNameScreen.SetActive(true);
    }

    public void OnClick_LeaveBtn()      //nút Leave
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(1);
    }

    public void OnClick_QuitGameBtn()   //nút Quit Game
    {
        alertQuit.SetActive(true);
    }

    public void OnClick_NoBtn()         //nút No
    {
        alertQuit.SetActive(false);
    }

    public void OnClick_YesBtn()        //nút Yes
    {
        Application.Quit();
    }

    public void OnClick_OkBtn()        //nút OK
    {
        alertRoom.SetActive(false);
    }


    public void OnClick_ReconnectBtn()
    {
        PhotonNetwork.ConnectUsingSettings(); //khi nhấn Connect_button sẽ kết nối tới Photon Server theo App ID tại PhotonServerSettings
        PhotonNetwork.LoadLevel(1);
    }

    public void OnChange_CreateNameRoom()    //hàm gọi khi giá trị ở ô Text Box nhập tên phòng thay đổi (tạo phòng)
    {
        if (createRoomTxt.text.Length > 0)    //kiểm tra tên nhập vào lớn hơn 2 kí tự
        {
            createRoomBtn.interactable = true; //button CreateRoomBtn sẽ enable //interactable: tương tác
        }
        else
            createRoomBtn.interactable = false;
    }

    public void OnChange_JoinNameRoom()    //hàm gọi khi giá trị ở ô Text Box nhập tên phòng thay đổi (tạo phòng)
    {
        if (joinRoomTxt.text.Length > 0)    //kiểm tra tên nhập vào lớn hơn 2 kí tự
        {
            joinRoomBtn.interactable = true; //button CreateRoomBtn sẽ enable //interactable: tương tác
        }
        else
            joinRoomBtn.interactable = false;
    }

    public void OnClick_CreateRoom()    //xảy ra khi nhấn Create Room Button
    {
        PhotonNetwork.CreateRoom(createRoomTxt.text, new RoomOptions { MaxPlayers = 10 }, null);  //tạo phòng mới với tên nhập ở TextBox với số lượng người chới tối đa là 10    
    }

    public void OnClick_JoinRoom()  //xảy ra khi nhấn Join Room Button
    {
        PhotonNetwork.JoinRoom(joinRoomTxt.text, null); //tham gia phòng với tên nhập ở TextBox
    }

    public override void OnJoinedRoom()
    {
        //print("Room Joned Sucess");
        PhotonNetwork.LoadLevel(2); //load tới SampleScene
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //print("Room Failed " + returnCode + " Message " + message); //hàm check báo OnCreateRoomFailed khi chạy thử game
        alertRoom.SetActive(true);
        alertRoom.transform.Find("Text").GetComponent<Text>().text = "The room name exist";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //print("Room Failed " + returnCode + " Message " + message); //hàm check báo JoinRoomFailed khi chạy thử game
        alertRoom.SetActive(true);
        alertRoom.transform.Find("Text").GetComponent<Text>().text = "Room full or not exist";
    }

    public void OnClick_StartMatch()    //xảy ra khi nhấn StartMatch Button
    {
        Debug.Log(PlayerPrefs.GetInt("selectedCharacter"));
        SpawnPlayer();
        inRoomScreen = GameObject.Find("Canvas").transform.Find("InRoomScreen").gameObject;
        inRoomScreen.SetActive(false);
        //PhotonNetwork.LoadLevel(3);
    }

    public void OnClick_BackToLobbyBtn()  //nút Leave
    {
        PhotonNetwork.LoadLevel(1);
    }
}
