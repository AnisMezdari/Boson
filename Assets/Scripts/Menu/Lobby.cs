using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Boson
{
    public class Lobby : MonoBehaviourPunCallbacks
    {


        public int nbPlayer = 1;
        public int indexPlayer = 1;

        public bool TriesToConnectToMaster;
        public bool TriesToConnectToRoom;
        public bool isReady = false;

        public GameObject UI_Menu;
        public GameObject playerInstantiate;

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
           
            if (playerInstantiate != null )
            {
                isReady = true;
            }

        }

        public void ConnectToMaster()
        {
            TriesToConnectToMaster = true;
            
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            TriesToConnectToMaster = false;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;
        }

        public void ConnectToRoom()
        {
            TriesToConnectToRoom = true; 
            PhotonNetwork.JoinRandomRoom();
            //PhotonNetwork.CreateRoom("Peter's Game 1"); 
            //PhotonNetwork.JoinRoom("Peter's Game 1");  
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            TriesToConnectToRoom = false;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            nbPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            indexPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            UI_Menu.GetComponent<UI_MainMenu>().InitiatePlayerNameText();
            SpawnPlayer(ref playerInstantiate);

        }

        void SpawnPlayer(ref GameObject playerInstantiate)
        {

            if (playerInstantiate != null)
            {
                PhotonNetwork.Destroy(playerInstantiate);
            }
            playerInstantiate = PhotonNetwork.Instantiate("Perso", new Vector3(0, 0, 1), Quaternion.identity);
            string playerNameInInput = UI_Menu.GetComponent<UI_MainMenu>().inputNamePlayer.text;
            playerInstantiate.GetComponent<UI_PlayerBoson>().SendNamePlayer(playerNameInInput);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            nbPlayer++;
            SpawnPlayer(ref playerInstantiate);
        }

         
    }
}


