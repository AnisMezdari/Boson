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
        transform.Translate(this.transform.up *Time.deltaTime * 10  , Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Zombie(Clone)")
        {
            //Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        if(collision.name == "wallDown" || collision.name == "wallRight" || collision.name == "wallUp" || collision.name == "wallLeft")
        {
            Destroy(this.gameObject);
        }
        
    }



}
