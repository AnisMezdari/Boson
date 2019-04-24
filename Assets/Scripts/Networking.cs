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


        public bool TriesToConnectToMaster;
        public bool TriesToConnectToRoom;

        // Use this for initialization
        void Start()
        { 
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
            startGame.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && TriesToConnectToRoom);
            nbPlayerVarText.text = nbPlayer + "";
        }

        public void OnClickConnectToMaster()
        {
            TriesToConnectToMaster = true;
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

        GameObject playerInstantiate;

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            //TriesToConnectToRoom = false;
            Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name);
            //SceneManager.LoadScene("Network");
            this.nbPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            SpawnPlayer(ref playerInstantiate);

        }

        void SpawnPlayer(ref GameObject playerInstantiate)
        {
            //GameObject playerinstantiate = Instantiate(player, new Vector3(0+(positionX*SCALE_X), 0+(positionY*SCALE_Y), 1), Quaternion.identity);
            if (playerInstantiate != null)
            {
                PhotonNetwork.Destroy(playerInstantiate);
            }
            playerInstantiate = PhotonNetwork.Instantiate("Perso", new Vector3(0, 0, 1), Quaternion.identity);
        }

        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            nbPlayer++;
            SpawnPlayer(ref playerInstantiate);
        }

        public void OnclikStartGame()
        {
            //SceneManager.LoadScene("Game");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Game");
            }
            

        } 
         
    }
}


