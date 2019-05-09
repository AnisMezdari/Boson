using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBoson : MonoBehaviourPun
{  
    public int health = 3;
    public int positionX;
    public int positionY;

    private Game game;
    public UI_PlayerBoson  ui_player;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != null)
        {
            game = GameObject.Find("GameScript").GetComponent<Game>();
            if (photonView.IsMine)
            {
                if (scene.name == "Game")
                {
                    ui_player = this.transform.GetComponent<UI_PlayerBoson>();
                    ui_player.ChangeHealth(health);
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {    
        if (photonView.IsMine)
        {
            if (game != null)
            {
                Camera.main.transform.position = new Vector3((positionX * game.SCALE_X), (positionY * game.SCALE_Y), -(game.INITIATE_POSITION));
            }
        }
    }

    [PunRPC]
    void SpawnPosition(int positionX, int positionY, float initatePosition = 0, int indexPlayer = 0, int spaceScale = 0)
    {
        this.transform.position = new Vector3((-(initatePosition) + (indexPlayer * spaceScale) ) + (positionX * game.SCALE_X), 0 + (positionY * game.SCALE_Y), 1);
        this.positionX = positionX;
        this.positionY = positionY;
    }

    [PunRPC]
    void SetPosition(int positionX ,int positionY)
    {
        this.positionX = positionX;
        this.positionY = positionY;
    }


    public void UseSpawnPlayerRPC(int positionX, int positionY, float initatePosition = 0, int indexPlayer = 0, int spaceScale = 0)
    {
        photonView.RPC("SpawnPosition", RpcTarget.All, positionX, positionY, initatePosition, indexPlayer, spaceScale);
    }

    public void UseSetPositionRPC(int positionX, int positionY)
    {
        photonView.RPC("SetPosition", RpcTarget.All, positionX, positionY);
    }


    /*
     * When player take a door
     * Left / right / top and botoom
     */
    public void TakeLeftDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x - 8, this.transform.position.y);
        positionX--;
        photonView.RPC("SetPosition", RpcTarget.All, positionX, positionY);
    }
    public void TakeRightDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x + 8, this.transform.position.y);
        positionX++;
        photonView.RPC("SetPosition", RpcTarget.All, positionX, positionY);
    }

    public void TakeTopDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 8);
        positionY--;
        photonView.RPC("SetPosition", RpcTarget.All, positionX, positionY);
    }

    public void TakeBottomDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 8);
        positionY++;
        photonView.RPC("SetPosition", RpcTarget.All, positionX, positionY);
    }



    public void Die()
    {
        PhotonNetwork.Destroy(this.photonView);
        game.CheckSurvivor();
        ui_player.Loose();
    }

    public void Win()
    {
        ui_player.Win();
        game.CheckSurvivor();
    }


}


