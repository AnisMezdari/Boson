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
    public int munitions= 6 ;
    public bool spectatorMode = false;
    public bool won = false;
    public int ranking = 0;
    public bool readyFornextGame = false;

    public PlayerBoson spectatorPlayer;

    public Game game;
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
            if (won && CheckSurvivor() < 1 && NumberWonSurvivor() < 2)
            {
                ui_player.EndGame();
            }
            if (readyFornextGame)
            {
                Camera.main.GetComponent<Spectator>().SetPlayer(this);
                Camera.main.GetComponent<Spectator>().SetSpectatorMode(false);
                readyFornextGame = false;
            }
        }
        if (!won)
        {
            this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1f);
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

    [PunRPC]
    public void SetState(int munitions, bool won )
    {
        this.munitions = munitions;
        this.won = won;
        health = 3;
        this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
    }


    public void UseSetStateRPC(int munitions, bool won)
    {
        photonView.RPC("SetState", RpcTarget.All, munitions, won);
    }


    /*
     * When player take a door
     * Left / right / top and botoom
     */
    public void TakeLeftDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x - 8, this.transform.position.y);
        positionX--;
        TakeDoor();
    }
    public void TakeRightDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x + 8, this.transform.position.y);
        positionX++;
        TakeDoor();
    }

    public void TakeTopDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 8);
        positionY--;
        TakeDoor();
    }

    public void TakeBottomDoor()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 8);
        positionY++;
        TakeDoor();
    }

    public void TakeDoor()
    {
        photonView.RPC("SetPosition", RpcTarget.All, positionX, positionY);
        int numberZombie = Random.Range(1, 5);
    }


    public void Die()
    {
        ranking = RankingCalculation();
        ui_player.SetRanking(ranking);
        ui_player.UI_Object.loose = true;
        if (CheckSurvivor() > 1)
        {
            ui_player.Loose();
        }
        else
        {
            if (CheckWonSurvivor())
            {
                ui_player.Loose();
            }
            else
            {
                
                ui_player.End();
            }
           
        }
        game.CheckSurvivor();
        PhotonNetwork.Destroy(this.photonView);
        Destroy(this.gameObject);
    }

    public int  RankingCalculation()
    {
        return (CheckSurvivor()) + NumberWonSurvivor();
    }

    public void Win()
    {
        
        if (CheckSurvivor() > 1)
        {
            ui_player.Win();
            photonView.RPC("SetState", RpcTarget.All, 6, true);
        }
        else
        {
            if (CheckWonSurvivor())
            {
                ui_player.Next();
                photonView.RPC("SetState", RpcTarget.All, 6, true);
                game.CheckSurvivor();
            }
            else
            {
                ui_player.EndGame();
                photonView.RPC("SetState", RpcTarget.All, 6, true);
            }
           
        }
           
 
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

    public bool CheckWonSurvivor()
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
        return counter > 0;
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


}


