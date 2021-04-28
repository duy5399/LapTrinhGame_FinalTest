﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.Text;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityStandardAssets.CrossPlatformInput;

public class DouxScript : MyPlayer
{
    public Sprite douxWon;
    public Sprite iceImgSkill;
    void Start()
    {
        //PhotonNetwork.SendRate = 20;          //Xác định số lần PhotonNetwork gửi đi 1 gói dữ liệu- càng cao càng ít độ trễ cho game nhưng tốn tài nguyên
        //PhotonNetwork.SerializationRate = 15; //xác định số lần hàm OnPhotonSerialize được gọi - càng cao càng ít độ trễ cho game nhưng tốn tài nguyên
        if (photonView.IsMine)  //PhotonView: đối tượng trên mạng (xác định bằng viewID)
        {
            isGrounded = true;
            rb = GetComponent<Rigidbody2D>();
            PhotonNetwork.LocalPlayer.SetScore(0);      //khởi tạo điểm nhân vật bằng 0
            nameText.text = PhotonNetwork.NickName;     //hiện tên Player của mình
            nameText.color = Color.yellow;

            anim = GetComponent<Animator>();
            joystick = FindObjectOfType<Joystick>();    //tìm Fixed Joytick gán vào joystick

            alertDeath = GameObject.Find("Canvas").transform.Find("AlertDeath").gameObject;    //tìm UI AlertDeath gán vào alertScreen
            alertDeath.SetActive(false);
            scoreBoard = GameObject.Find("Canvas").transform.Find("ScoreBoard").gameObject;     //tìm UI ScoreBoard gán vào scoreBoard
            menu = GameObject.Find("Canvas").transform.Find("Menu").gameObject;                 //tìm UI Menu gán vào menu
            //awardScreen = GameObject.Find("Canvas").transform.Find("AwardScreen").gameObject;

            //winnerScreen = GameObject.Find("Canvas").transform.Find("AlertWinner").gameObject;

            //Camera tập trung vào Player
            playerCamera = GameObject.Find("Main Camera");   //tìm đối tượng Main Camera

            //sceneCamera.SetActive(false);   //tắt Main Camera   
            playerCamera.SetActive(true);   //bật Player Camera

            jumpBtn = GameObject.Find("Canvas").transform.Find("ButtonJump").GetComponent<Button>();
            kickBtn = GameObject.Find("Canvas").transform.Find("ButtonKick").GetComponent<Button>();
            skillBtn = GameObject.Find("Canvas").transform.Find("ButtonSkill").GetComponent<Button>();
            skillBtn.GetComponent<Image>().sprite = iceImgSkill;
        }
        else
        {
            nameText.text = pv.Owner.NickName;  //pv: 1 đối tượng trên mạng qua PhontonView - bất cứ ai là chủ sở hữu pv thì sẽ lấy nametext của pv đó
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)  //PhotonView: đối tượng trên mạng (xác định bằng viewID)
        {
            CheckDead();
            ProcessInputs();    //hàm di chuyển nhân vật
            Jump();             //hàm nhảy
            Kick();             //hàm tấn công (Kick)
            Skill();              //hàm húc
            Respawn();          //hàm chết và hồi sinh
            ShowScore();        //hàm để xem bảng điểm
            ShowMenu();
            Victory();
            //pv.RPC("Victory", RpcTarget.OthersBuffered);
        }
        else
        {
            SmoothMovement();   //nếu không phải người chơi cục bộ - PC1 có Player Y và Duy không điều khiển Player Y mà là một người khác (Hoài)
                                //=>vị trí Player Y sẽ di chuyển từ một người khác (Hoài điều khiển từ PC2)
            SmoothHealth();
        }
    }

    public void Skill()
    {
        GameObject iceStack;
        if (CrossPlatformInputManager.GetButtonDown("Skill") && !cooldown && skillBtn.interactable == true)
        {
            if (sr.flipX == true)
            {
                iceStack = PhotonNetwork.Instantiate(effectPrefab.name, kickSpawnLeft.position, Quaternion.identity);   //Instantiate: khởi tạo Bullet
                iceStack.GetComponent<PhotonView>().RPC("ChangeDirectionOfIce", RpcTarget.AllBuffered);
                //iceStack.GetComponent<IceEffect>().left = true;
                //iceStack.GetComponent<PhotonView>().RPC("updatePosition", RpcTarget.AllBuffered, gameObject.transform.position);
            }
            else
            {
                iceStack = PhotonNetwork.Instantiate(effectPrefab.name, kickSpawnRight.position, Quaternion.identity);   //Instantiate: khởi tạo Bullet
                //iceStack.GetComponent<IceEffect>().left = false;
            }
            cooldown = true;
            musicSource[0].Play();
            skillDelay = 1f;
        }
        if (cooldown)
        {
            if (skillDelay > 0)
            {
                skillDelay -= Time.deltaTime;
                skillBtn.transform.Find("Text").GetComponent<Text>().text = "" + Math.Round(skillDelay, 1);
                //healEffect.GetComponent<HealScript>().updatePosition(gameObject.transform.position);
            }
            else
            {
                cooldown = false;
                skillBtn.transform.Find("Text").GetComponent<Text>().text = "Skill";
            }
        }
    }

    [PunRPC]
    public override void SetAward(string nameWinner)
    {
        awardScreen = GameObject.Find("Canvas").transform.Find("AwardScreen").gameObject;
        awardScreen.GetComponent<Image>().sprite = douxWon;
        awardScreen.SetActive(true);                 //hiện màn hình giành cho người chiến thắng
        awardScreen.transform.Find("NameWinnerTxt").GetComponent<Text>().text = nameWinner;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName.ToString() != nameWinner)
            {
                PhotonNetwork.DestroyPlayerObjects(player);
            }
        }
        Destroy(gameObject);
    }

    public override void Victory()       //hàm kiểm tra điểm số (người chiến thắng)
    {
        if (photonView.Owner.GetScore() >= 2)    //nếu điểm số đạt số điểm yêu cầu
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;       //khóa phòng chơi
            PhotonNetwork.CurrentRoom.IsVisible = false;    //ẩn phòng chơi   
            //PhotonNetwork.LoadLevel(4);  
            pv.RPC("SetAward", RpcTarget.AllBuffered, photonView.Owner.NickName.ToString());     //gọi hàm cho những người còn lại (người thất bại)
            SetAward(photonView.Owner.NickName);
        }
    }
}
