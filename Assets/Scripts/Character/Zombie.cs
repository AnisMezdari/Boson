using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviourPun {


    public float movingSpeed;
    public int Xposition;
    public int Yposition;
    private Transform target;
    private bool followPlayer;
    private GameObject playerFollowed;

    // Use this for initialization
    void Start () {

        followPlayer = false;
    }

    // Update is called once per frame
    void Update () {

        if(followPlayer)
        {
            // if the player who is being followed is dead
            if (target == null)
            {
                ChoosePlayer();

            }else if (playerFollowed != null  &&  playerFollowed.GetComponent<PlayerBoson>().won)
            {
                ChoosePlayer();
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
                FollowPlayer();
            }
                
        }
        else
        {
            ChoosePlayer();
        }

        
    }

    private void TakeDoor(int index)
    {
        GameObject room = GameObject.Find("Room_"+ Xposition + "_" + Yposition);
        this.target = room.transform.GetChild(index).transform;
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

        if (coll.name == "DoorLeft")
        {
            this.transform.position = new Vector3(this.transform.position.x - 6, this.transform.position.y, 0);
            Xposition--;
        }
        if (coll.name == "DoorDown")
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 8, 0);
            Yposition++;
        }
        if (coll.name == "DoorRight")
        {
            this.transform.position = new Vector3(this.transform.position.x + 6, this.transform.position.y, 0);
            Xposition++;
        }
        if (coll.name == "DoorTop")
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 8, 0);
            Yposition--;
        }

        if (coll.name == "Bullet(Clone)")
        {
            Destroy(this.gameObject);
        }


    }

    void FollowPlayer()
    {
        if(playerFollowed == null)
        {
            followPlayer = false;
            return;
        }
        if (playerFollowed.GetComponent<PlayerBoson>().positionX < this.Xposition)
        {
            TakeDoor(1);
        } else if (playerFollowed.GetComponent<PlayerBoson>().positionX > this.Xposition)
        {
            TakeDoor(3);
        } else if (playerFollowed.GetComponent<PlayerBoson>().positionY < this.Yposition)
        {
            TakeDoor(4);
        } else if(playerFollowed.GetComponent<PlayerBoson>().positionY > this.Yposition)
        {
            TakeDoor(2);
        }
        else
        {
            this.target = playerFollowed.transform;
        }
    }
    private void ChoosePlayer()
    {
        List<GameObject> listPlayerInSameRoom = ListPlayerSameRoom();
        if (listPlayerInSameRoom.Count > 0)
        {
            AssignPlayerToTarget(listPlayerInSameRoom);

        }
        else
        {
            this.target = null;
            followPlayer = false;
        }
    }

     private void AssignPlayerToTarget(List<GameObject> listPlayerInSameRoom)
     {
        int indexPlayerRandom = Random.Range(0, listPlayerInSameRoom.Count);
        GameObject targetPlayer = listPlayerInSameRoom[indexPlayerRandom];
        this.target = targetPlayer.transform;
        this.playerFollowed = targetPlayer;
        StartCoroutine(WaitBeforeChargeplayer());
        //followPlayer = true;
     }


    List<GameObject> ListPlayerSameRoom()
    {
        GameObject[] listPlayer = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> listPlayerInSameRoom = new List<GameObject>();
        for (int i = 0; i < listPlayer.Length; i++)
        {
            if (PlayerSameRoom(listPlayer[i]))
            {
                if (!listPlayer[i].GetComponent<PlayerBoson>().won)
                {
                    listPlayerInSameRoom.Add(listPlayer[i]);
                }
               
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


    IEnumerator WaitBeforeChargeplayer()
    {
        yield return new WaitForSeconds(1);
        followPlayer = true;
    }

}
