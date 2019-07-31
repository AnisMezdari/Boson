using Photon.Pun;
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
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loose && CheckSurvivor() < 1 && NumberWonSurvivor() < 2)
        {
            endText.gameObject.SetActive(true);
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
}
