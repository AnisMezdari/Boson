using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviourPunCallbacks
{

	public GameObject map;
	public GameObject player;
	public GameObject zombie;

	public int widthMap = 4;
	public int heightMap = 4;
	public bool randomDelete = false;
	public int minimumRoom = 3;
	public int numberZombie = 5;

	public  int SCALE_X = 34;
    public  int SCALE_Y = -17;

	// Use this for initialization
	void Start () {
 
        while (InitateMap(widthMap,heightMap,randomDelete) < minimumRoom);
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnZombie(numberZombie);
        }
	}

	int InitateMap(int largeur,int hauteur,bool randomDelete){
		int counterRoom = 0;
		int exitX = Random.Range(1, widthMap);
		int exitY= Random.Range(1, heightMap);
		for(int j=0;j<hauteur;j++){
			for(int i=0;i<largeur;i++){
				GameObject mapInstantiate = Instantiate(map, new Vector3(0+(i*SCALE_X), 0+(j*SCALE_Y), 1), Quaternion.identity);
				mapInstantiate.name = "Room_"+i+"_"+j;
				InitiateDoor(mapInstantiate,i,j);
				if(exitX == i && exitY == j){
					InitiateExit(mapInstantiate);
				}
				counterRoom++;
			}
		}
		return counterRoom;
	}

	void  InitiateExit(GameObject map){
		map.transform.GetChild(5).gameObject.SetActive(true);
	}

	void  InitiateDoor(GameObject map, int i, int j){
			int right = 3;
			int down = 2;
			int left = 1;
			int up = 4;

			if(j == 0){
				map.transform.GetChild(down).gameObject.SetActive(true);
			}
			if(i == 0){
				map.transform.GetChild(right).gameObject.SetActive(true);
			}
			if( i > 0 && i< (widthMap-1) ){
				map.transform.GetChild(left).gameObject.SetActive(true);
				map.transform.GetChild(right).gameObject.SetActive(true);
			}
			if(j > 0 &&  j < (heightMap-1)) {
				map.transform.GetChild(up).gameObject.SetActive(true);
				map.transform.GetChild(down).gameObject.SetActive(true);
			}
			if(i == (widthMap-1) ){
				map.transform.GetChild(left).gameObject.SetActive(true);
			}
			if(j == (heightMap-1) ){
				map.transform.GetChild(up).gameObject.SetActive(true);
			}
	}



    void SpawnZombie(int numberZombie){

		for(int i =0; i<numberZombie;i++){
			int positionX = Random.Range(1, widthMap);
			int positionY = Random.Range(1, heightMap);
            GameObject zombieInstantiate = PhotonNetwork.InstantiateSceneObject("Zombie", new Vector3(0 + (positionX * SCALE_X), 0 + (positionY * SCALE_Y), 1), Quaternion.identity);
			MovingZombie movingScript = zombieInstantiate.GetComponent<MovingZombie>();
			movingScript.Xposition = positionX;
			movingScript.Yposition = positionY;
		}

	}


    // Update is called once per frame
    void Update () {

	}
}
