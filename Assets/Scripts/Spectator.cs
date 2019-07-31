using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{

    bool spectatorMode = false;

    PlayerBoson playerObserved;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spectatorMode)
        {
            if(playerObserved != null && !playerObserved.won) {
                Camera.main.transform.position = new Vector3((playerObserved.positionX * playerObserved.game.SCALE_X),
                                                             (playerObserved.positionY * playerObserved.game.SCALE_Y), 
                                                            -(playerObserved.game.INITIATE_POSITION));
            }
            else
            {
                LaunchSpectatorMode();
            }
           
        }
    }

    public void LaunchSpectatorMode()
    {
        if (GameObject.Find("Perso(Clone)") != null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    
            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].GetComponent<PlayerBoson>().won)
                {
                    playerObserved = players[i].GetComponent<PlayerBoson>();
                }
            }
            spectatorMode = true;
        }
      
    }



}
