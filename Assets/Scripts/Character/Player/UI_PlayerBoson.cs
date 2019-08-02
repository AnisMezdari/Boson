using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_PlayerBoson: MonoBehaviourPun
{

    public string namePlayerBoson = "";

    public UI_Objects UI_Object;

    private PlayerBoson player;

    public Text health;
    public Text munitions;


    void Start()
    {
        player = this.transform.GetComponent<PlayerBoson>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != null)
        {
            if (photonView.IsMine)
            {
                if (scene.name == "Game")
                {
                    UI_Object = GameObject.FindGameObjectWithTag("Script_UI_Player").GetComponent<UI_Objects>();
                    ChangeMunitions(player.munitions);
                }
            }

        }
    }


    void Update()
    {
        if (this != null)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = namePlayerBoson;

        }
        
    }

    public void SendNamePlayer(string namePlayerBoson)
    {
        photonView.RPC("SetNamePlayer", RpcTarget.All, namePlayerBoson);
    }

    [PunRPC]
    public void SetNamePlayer(string namePlayerBoson)
    {
        this.namePlayerBoson = namePlayerBoson;
    }



    public void OnclickExitButtonu()
    {

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");

    }

    public void ChangeHealth(int health)
    {
        if (UI_Object != null)
        {
            UI_Object.health.text = health + "";
        }
        
    }
    public void ChangeMunitions(int munitions)
    {
         UI_Object.munitions.text = munitions + "x";
    }

    public void Win()
    {
        UI_Object.winText.gameObject.SetActive(true);
    }

    public void Loose()
    {
        UI_Object.looseText.gameObject.SetActive(true);
    }

    public void End()
    {
        UI_Object.endText.gameObject.SetActive(true);
    }

    public void Next()
    {
        UI_Object.nextText.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        UI_Object.winGametext.gameObject.SetActive(true);
    }

    public void SetRanking(int ranking)
    {
        UI_Object.SetRanking(ranking);
    }
}
