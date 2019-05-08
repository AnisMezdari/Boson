using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPlayer : MonoBehaviourPun
{
    private PlayerBoson player;
    void Start()
    {
        player = this.GetComponent<PlayerBoson>();
    }
        void OnTriggerEnter2D(Collider2D coll)
    {

        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {

            if (coll.name == "DoorLeft")
            {
                player.TakeLeftDoor();
            }
            
            if (coll.name == "DoorDown")
            {
                player.TakeBottomDoor();
            }
            if (coll.name == "DoorRight")
            {
                player.TakeRightDoor();
            }
            if (coll.name == "DoorTop")
            {
                player.TakeTopDoor();
            }
            if (coll.name == "Escape(Clone)")
            {
                player.Win();
            }
            
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            if (collision.transform.name == "Zombie(Clone)")
            {
                player.health--;
                player.ui_player.ChangeHealth(player.health);

                if (player.health == 0)
                {
                    player.Die();
                }
            }
        }
    }
}
