using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBoson : MonoBehaviourPun
{
  
    public float movingSpeed;

    public string inputFront;
    public string inputBack;
    public string inputRight;
    public string inputLeft;
    public Joystick joystick;
    public int health = 3;

    public int positionX;
    public int positionY;

    private Game game;

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
            Camera.main.aspect = 2960f / 1440f;
            game = GameObject.Find("GameScript").GetComponent<Game>();
            if (photonView.IsMine)
            {
                if (scene.name == "Game")
                {
                    joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
                    Text healthStr = GameObject.Find("Health").GetComponent<Text>();
                    healthStr.text = this.health + "";
                }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {    
        if (photonView.IsMine)
        {
            InputMoving();

            if (game != null)
            {
                Camera.main.transform.position = new Vector3((positionX * game.SCALE_X), (positionY * game.SCALE_Y), -10);
            }
        }
    }

    [PunRPC]
     void SpawnPlayer(int positionX, int positionY, int compteur)
    {
        this.transform.position = new Vector3((-10 + (compteur*3) ) + (positionX * game.SCALE_X), 0 + (positionY * game.SCALE_Y), 1);
        this.positionX = positionX;
        this.positionY = positionY;
    }

    [PunRPC]
    void SetPositionX(int positionX)
    {
        this.positionX = positionX;
    }

    [PunRPC]
    void SetPositionY(int positionY)
    {
        this.positionY = positionY;
    }

    private void InputMoving()
    {
        transform.Translate(0,  joystick.Vertical * movingSpeed * Time.deltaTime,0);
    	transform.Translate( joystick.Horizontal * movingSpeed * Time.deltaTime, 0,0 );
    
    }

    public void UseSpawnPlayerRPC(int positionX,int positionY,int compteur)
    {
        photonView.RPC("SpawnPlayer", RpcTarget.All, positionX, positionY,compteur);
    }


   /* private void InputMoving()
    {
        // Z
        if (Input.GetKey(inputFront))
        {
            transform.Translate(0, movingSpeed * Time.deltaTime, 0);

        }
        // S
        if (Input.GetKey(inputBack))
        {
            transform.Translate(0, -movingSpeed * Time.deltaTime, 0);

        }
        // q
        if (Input.GetKey(inputLeft))
        {
            transform.Translate(-movingSpeed * Time.deltaTime, 0, 0);

        }
        // d
        if (Input.GetKey(inputRight))
        {
            transform.Translate(movingSpeed * Time.deltaTime, 0, 0);

        }


    }*/

    void OnTriggerEnter2D(Collider2D coll)
    {

        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {

            if (coll.name == "DoorLeft")
            {
                this.transform.position = new Vector2(this.transform.position.x - 6, this.transform.position.y);
                positionX--;
                photonView.RPC("SetPositionX", RpcTarget.All, positionX);
            }
            if (coll.name == "DoorDown")
            {
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 8);
                positionY++;
                photonView.RPC("SetPositionY", RpcTarget.All, positionY);
            }
            if (coll.name == "DoorRight")
            {
                this.transform.position = new Vector2(this.transform.position.x + 6, this.transform.position.y);
                positionX++;
                photonView.RPC("SetPositionX", RpcTarget.All, positionX);
            }
            if (coll.name == "DoorTop")
            {
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 8);
                positionY--;
                photonView.RPC("SetPositionY", RpcTarget.All, positionY);
            }
            if (coll.name == "Escape(Clone)")
            {
                Win();
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            if (collision.transform.name == "Zombie(Clone)")
            {
                this.health--;
                Text healthStr = GameObject.Find("Health").GetComponent<Text>();
                healthStr.text = this.health + "";
                if (this.health == 0)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        PhotonNetwork.Destroy(this.photonView);
        CheckSurvivor();
        this.game.looseText.gameObject.SetActive(true);
    }

    private void Win()
    {
        this.game.winText.gameObject.SetActive(true);
    }

    private void CheckSurvivor()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length == 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {

    }

}


