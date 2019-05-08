using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviourPunCallbacks
{
    /*
     * All varaibles of UI main menu
     *  BtnConnectMaster -> button to connect of master
     *  BtnConnectRoom -> button to join a random room
     *  nbPlayerText  -> text -> "number of player : "
     *  nbPlayerVarText -> number variable of player in text
     *  startGame -> button to launch one game
     *  inputNamePlayer -> input to change the player name
    */
    public Button btnConnectMaster;
    public Button btnConnectRoom;
    public Text nbPlayerText;
    public Text nbPlayerVarText;
    public Button startGame;
    public InputField inputNamePlayer;

    // Looby game object to get all variables
    public GameObject lobby;
    private Boson.Lobby lobbyScript;


    void Start()
    {
        lobbyScript = lobby.GetComponent<Boson.Lobby>();
    }


    /*
     * Hide ui element depends of situation
     * if in main menu so we display BtnConnectMaster
     * If BtnConnectMaster is pressed -> hide BtnConnectMaster and display BtnConnectRoom
     * etc..
    */
    void Update()
    {
        btnConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !lobbyScript.TriesToConnectToMaster);
        btnConnectRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !lobbyScript.TriesToConnectToMaster && !lobbyScript.TriesToConnectToRoom);
        nbPlayerText.gameObject.SetActive(PhotonNetwork.IsConnected && !lobbyScript.TriesToConnectToMaster && lobbyScript.TriesToConnectToRoom);
        nbPlayerVarText.gameObject.SetActive(PhotonNetwork.IsConnected && !lobbyScript.TriesToConnectToMaster && lobbyScript.TriesToConnectToRoom);
        inputNamePlayer.gameObject.SetActive(PhotonNetwork.IsConnected && !lobbyScript.TriesToConnectToMaster && lobbyScript.TriesToConnectToRoom);
        startGame.gameObject.SetActive(PhotonNetwork.IsConnected && !lobbyScript.TriesToConnectToMaster && lobbyScript.TriesToConnectToRoom && lobbyScript.isReady);

        nbPlayerVarText.text = lobbyScript.nbPlayer + "";
    }

    public void OnClickPlayButton()
    {
        GameObject.Find("Canvas_MainMenu").SetActive(false);
        lobbyScript.ConnectToMaster();
    }

    public void InitiatePlayerNameText()
    {
        inputNamePlayer.text = "Player " + lobbyScript.indexPlayer;
    }

    public void OnClickConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        lobbyScript.ConnectToRoom();
    }

    public void OnChangeInputNamePlayer()
    {
        string playerNameInInput = inputNamePlayer.text;
        if (lobbyScript.playerInstantiate != null)
        {
            lobbyScript.playerInstantiate.GetComponent<UI_PlayerBoson>().SendNamePlayer(playerNameInInput);
        }

    }

    public void OnclikStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }

    }


}
