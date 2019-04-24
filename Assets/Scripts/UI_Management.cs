using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Management : MonoBehaviourPun
{

    public string namePlayerBoson = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this != null)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = namePlayerBoson;
        }
        
    }

    
    public void SendNamePlayer(string namePlayerBoson)
    {
        photonView.RPC("SetNamePlayer", RpcTarget.All, namePlayerBoson);
    }

    [PunRPC]
    public void SetNamePlayer(string namePlayerBoson)
    {
        this.namePlayerBoson = namePlayerBoson;
    }
}
