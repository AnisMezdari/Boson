using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviourPun {


    public float movingSpeed;
    public int Xposition;
    public int Yposition;
    private Transform target;
    private Transform oldPosition;
    private Vector3 direction;
    private bool followPlayer;
    private GameObject playerFollowed;

    // Use this for initialization
    void Start () {
        if (PhotonNetwork.IsMasterClient)
        {
            followPlayer = false;
            this.oldPosition = this.transform;
            TakeDoor();
        }
    }

    // Update is called once per frame
    void Update () {
        if (PhotonNetwork.IsMasterClient)
        {
            if(target == null)
            {
                TakeDoor();
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
                FollowPlayer();
            }
        }
    }

    private void TakeDoor()
    {
        GameObject room = GameObject.Find("Room_"+ Xposition + "_" + Yposition);

        List<int> indexDoor = getDoorNumber(room);
        int indexInTableRandom = Random.Range(1,indexDoor.Count+1);
        this.target = room.transform.GetChild(indexDoor[indexInTableRandom-1]).transform;
        this.oldPosition = this.transform;
    }

    private List<int> getDoorNumber(GameObject room)
    {
        List<int> doorList = new List<int>();
        for (int i = 1; i < 5; i++)
        {
            GameObject door = room.transform.GetChild(i).gameObject;
            if (door.activeSelf == true)
            {
                doorList.Add(i);
            }
        }
        return doorList;
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (PhotonNetwork.IsMasterClient)
        {
            bool takeDoor = false;
            if (coll.name == "DoorLeft")
            {
                this.transform.position = new Vector3(this.transform.position.x - 6, this.transform.position.y, 0);
                takeDoor = true;
                Xposition--;
            }
            if (coll.name == "DoorDown")
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 8, 0);
                takeDoor = true;
                Yposition++;
            }
            if (coll.name == "DoorRight")
            {
                this.transform.position = new Vector3(this.transform.position.x + 6, this.transform.position.y, 0);
                takeDoor = true;
                Xposition++;
            }
            if (coll.name == "DoorTop")
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 8, 0);
                takeDoor = true;
                Yposition--;
            }
            if (takeDoor)
            {
                TakeDoor();
            }
        }
       
    }

    void FollowPlayer(){

        List<GameObject> listPlayerInSameRoom = ListPlayerSameRoom();
        if (listPlayerInSameRoom.Count == 0 )
        {

            if (followPlayer)
            {
                TakeDoor();
                followPlayer = false;
            }
         
        }
        else
        {

            if (!followPlayer)
            {
                ChoosePlayer(listPlayerInSameRoom);
            }
            else
            {
                if (!PlayerSameRoom(this.playerFollowed))
                {
                    ChoosePlayer(listPlayerInSameRoom);
                    
                }
            }
        }
    }

    private void ChoosePlayer(List<GameObject> listPlayerInSameRoom)
    {
        int indexPlayerRandom = Random.Range(0, listPlayerInSameRoom.Count);
        GameObject targetPlayer = listPlayerInSameRoom[indexPlayerRandom];
        this.target = targetPlayer.transform;
        this.playerFollowed = targetPlayer;
        followPlayer = true;
    }


    List<GameObject> ListPlayerSameRoom()
    {
        GameObject[] listPlayer = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> listPlayerInSameRoom = new List<GameObject>();
        for (int i = 0; i < listPlayer.Length; i++)
        {
            if (PlayerSameRoom(listPlayer[i]))
            {
                listPlayerInSameRoom.Add(listPlayer[i]);
            }
        }
        return listPlayerInSameRoom;
    }

    bool PlayerSameRoom(GameObject player)
    {
        int playerPositionX = player.GetComponent<PlayerBoson>().positionX;
        int playerPositionY = player.GetComponent<PlayerBoson>().positionY;

        if (playerPositionX == this.Xposition && playerPositionY == this.Yposition)
        {
            return true;
        }
        return false;
        
    }


}
