using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPlayer : MonoBehaviourPun
{
    private PlayerBoson player;
    public bool isInvincible;
    void Start()
    {
        player = this.GetComponent<PlayerBoson>();
        isInvincible = false;
    }
    private void Update()
    {
        if (!isInvincible)
        {
            CancelInvoke("CharacterBlink");
            this.GetComponent<Renderer>().enabled = true;
        }
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
                if (!isInvincible)
                {
                    player.health--;
                    player.ui_player.ChangeHealth(player.health);
                    //photonView.RPC("TemporaryInvincible", RpcTarget.All);
                    TemporaryInvincible();
                }

                if (player.health == 0)
                {
                    player.Die();
                }
            }
        }
        if (PhotonNetwork.IsConnected && collision.transform.name == "Zombie(Clone)")
        {
            if (!isInvincible)
            {
                TemporaryInvincible();
            }
        }
    }

    private void TemporaryInvincible()
    {
        StartCoroutine(WaitAfterReceiveAttack());
        InvokeRepeating("CharacterBlink", 0, 0.2f);
    }

    IEnumerator WaitAfterReceiveAttack()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3);
        isInvincible = false;
       
    }
    private void CharacterBlink()
    {
        this.GetComponent<Renderer>().enabled = !this.GetComponent<Renderer>().enabled;
    }
}
