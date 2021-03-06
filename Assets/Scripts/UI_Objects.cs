﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Objects : MonoBehaviourPun
{

    public GameObject looseText;
    public GameObject winText;
    public GameObject endText;
    public GameObject nextText;
    public GameObject winGametext;

    public Text health;
    public Text munitions;

    public bool loose = false;

    private Spectator spectator;
    private GameObject joystick;
    private GameObject shotButton;

    // Start is called before the first frame update
    void Start()
    {
        spectator = Camera.main.GetComponent<Spectator>();
        joystick = GameObject.Find("Fixed Joystick");
        shotButton = GameObject.Find("Shot button");
    }

    // Update is called once per frame
    void Update()
    {
        if (loose && CheckSurvivor() < 1 && NumberWonSurvivor() < 2)
        {
            endText.gameObject.SetActive(true);
        }
        if (spectator != null && spectator.spectatorMode)
        {
            joystick.GetComponent<CanvasGroup>().alpha = 0;
            shotButton.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            joystick.GetComponent<CanvasGroup>().alpha = 1;
            shotButton.GetComponent<CanvasGroup>().alpha = 1;
        }

    }
    public void OnclikSpactatorMode()
    {
        looseText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        Camera.main.GetComponent<Spectator>().LaunchSpectatorMode();
    }

    public void OnclickExitButtonu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");

    }

    public void HideNextPannel()
    {
        nextText.gameObject.SetActive(false);
    }

    public int NumberWonSurvivor()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int counter = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerBoson>().won)
            {
                counter++;
            }
        }
        return counter;
    }

    public int CheckSurvivor()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int counter = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<PlayerBoson>().won)
            {
                counter++;
            }
        }
        return counter;
    }

    public void SetRanking(int ranking)
    {
        endText.transform.GetChild(3).GetComponent<Text>().text = ranking + "";
    }
}
