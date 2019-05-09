using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Action : MonoBehaviourPun
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y);
        transform.Translate(this.transform.up *Time.deltaTime * 10  , Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Zombie(Clone)")
        {
            PhotonNetwork.Destroy(collision.gameObject);
            PhotonNetwork.Destroy(this.gameObject);
        }
        Debug.Log(collision.name);
        if(collision.name == "wallDown" || collision.name == "wallRight" || collision.name == "wallUp" || collision.name == "wallLeft")
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
