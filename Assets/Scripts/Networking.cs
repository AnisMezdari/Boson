using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Boson
{
    public class Networking : MonoBehaviourPunCallbacks
    {
        public Button BtnConnectMaster;
        public Button BtnConnectRoom;
        public Text nbPlayerText;
        public int nbPlayer = 1;
        public Text nbPlayerVarText;
        public Button startGame;
        public InputField inputNamePlayer;


        public bool TriesToConnectToMaster;
        public bool TriesToConnectToRoom;

        GameObject playerInstantiate;

        private bool isReady = false;
        private int indexPlayer = 1;

        // Use this for initialization
        void Start()
        {
            Camera.main.aspect = 2960f / 1440f;
            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;
        }

        // Update is called once per frame
        void Update()
        {
            BtnConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !TriesToConnectToMaster);
            BtnConnectRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);

            nbPlayerText.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && TriesToConnectToRoom);
            nbPlayerVarText.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && TriesToConnectToRoom);
            inputNamePlayer.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && TriesToConnectToRoom);
            startGame.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && TriesToConnectToRoom && isReady);
            
            nbPlayerVarText.text = nbPlayer + "";
            if (playerInstantiate != null )
            {
                isReady = true;
            }
        }

        public void OnClickConnectToMaster()
        {
            TriesToConnectToMaster = true;
            GameObject.Find("Canvas_MainMenu").SetActive(false);
            //PhotonNetwork.ConnectToMaster(ip,port,appid); //manual connection
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();           //automatic connection based on the config file in Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            TriesToConnectToMaster = false;
            Debug.Log("Connected to Master!");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;
            Debug.Log(cause);
        }

        public void OnClickConnectToRoom()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            TriesToConnectToRoom = true;
            //PhotonNetwork.CreateRoom("Peter's Game 1"); //Create a specific Room - Error: OnCreateRoomFailed
            //PhotonNetwork.JoinRoom("Peter's Game 1");   //Join a specific Room   - Error: OnJoinRoomFailed  
            PhotonNetwork.JoinRandomRoom();               //Join a random Room     - Error: OnJoinRandomRoomFailed  
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            base.OnJoinRandomFailed(returnCode, message);
            //no room available
            //create a room (null as a name means "does not matter")
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log("error creation room => " + message);
            base.OnCreateRoomFailed(returnCode, message);
            TriesToConnectToRoom = false;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            nbPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            indexPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            inputNamePlayer.text = "Player " + indexPlayer;
            SpawnPlayer(ref playerInstantiate);

        }

        void SpawnPlayer(ref GameObject playerInstantiate)
        {

            if (playerInstantiate != null)
            {
                PhotonNetwork.Destroy(playerInstantiate);
            }
            playerInstantiate = PhotonNetwork.Instantiate("Perso", new Vector3(0, 0, 1), Quaternion.identity);
            //playerInstantiate.GetComponent<UI_Management>().namePlayerBoson = "Player " + indexPlayer;
            string playerNameInInput = inputNamePlayer.text;
            playerInstantiate.GetComponent<UI_Management>().SendNamePlayer(playerNameInInput);

        }

        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            nbPlayer++;
            SpawnPlayer(ref playerInstantiate);
        }

        public void OnclikStartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {  
                PhotonNetwork.LoadLevel("Game");
            }    

        } 

        public void OnChangeInputNamePlayer()
        {
            string playerNameInInput = inputNamePlayer.text;
            if (playerInstantiate != null)
            {
                playerInstantiate.GetComponent<UI_Management>().SendNamePlayer(playerNameInInput);
            }
            
        }
         
    }
}


