using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public int MINIMUM_DISTANCE_ESCAPE = 1;

    public float SCALE_X = 34;
    public int SCALE_Y = -17;

    private int initialPlayerPositionX;
    private int initialPlayerPositionY;

    public GameObject looseText;
    public GameObject winText;

    // Use this for initialization
    void Start () {
        while (InitateMap(widthMap,heightMap,randomDelete) < minimumRoom);
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayers();
            InitiateExit();
            SpawnZombie(numberZombie);
            Camera.main.aspect = 2960f / 1440f;
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
				counterRoom++;
			}
		}
		return counterRoom;
	}

	void  InitiateExit(){

        GameObject randomPlayer = GameObject.Find("Perso(Clone)");
        int positionXPlayer = randomPlayer.GetComponent<PlayerBoson>().positionX;
        int positionYPlayer = randomPlayer.GetComponent<PlayerBoson>().positionY;
        float distance;
        int positionX;
        int positionY;
        do
        {
            positionX = Random.Range(1, widthMap);
            positionY = Random.Range(1, heightMap);
            distance = Mathf.Abs((float)positionX - positionXPlayer) + Mathf.Abs((float)positionY - positionYPlayer);
        } while (distance < MINIMUM_DISTANCE_ESCAPE);

        for (int j = 0; j < heightMap; j++)
        {
            for (int i = 0; i < widthMap; i++)
            {
                if (positionX == i && positionY == j)
                {
                    PhotonNetwork.Instantiate("Escape", new Vector3(0 + (i * SCALE_X), 0 + (j * SCALE_Y), 1), Quaternion.identity);
                }

            }
        }
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
        int positionX;
        int positionY;
        int positionXPlayer;
        int positionYPlayer;
        for (int i =0; i<numberZombie;i++){
			
            do
            {
                GameObject randomPlayer = GameObject.Find("Perso(Clone)");
                positionXPlayer = randomPlayer.GetComponent<PlayerBoson>().positionX;
                positionYPlayer = randomPlayer.GetComponent<PlayerBoson>().positionY;

                positionX = Random.Range(1, widthMap);
                positionY = Random.Range(1, heightMap);

            } while (positionX == positionXPlayer && positionY == positionYPlayer);

            GameObject zombieInstantiate = PhotonNetwork.InstantiateSceneObject("Zombie", new Vector3(0 + (positionX * SCALE_X), 0 + (positionY * SCALE_Y), 1), Quaternion.identity);
            Zombie movingScript = zombieInstantiate.GetComponent<Zombie>();
			movingScript.Xposition = positionX;
			movingScript.Yposition = positionY;
		}

	}

    private void SpawnPlayers()
    {
        int[] indexWidth = { 0, this.widthMap - 1};
        int[] indexHeight = { 0, this.heightMap - 1 };

        int randomSide = Random.Range(0, 2);
        
        if (randomSide == 0)
        {
            this.initialPlayerPositionY = Random.Range(0, this.heightMap);
            this.initialPlayerPositionX = indexWidth[Random.Range(0, 2)];
            Debug.Log(initialPlayerPositionX);
        }
        else
        {
            this.initialPlayerPositionY = indexHeight[Random.Range(0, 2)];
            this.initialPlayerPositionX = Random.Range(0, this.widthMap);
        }
        SetPostionPlayers(initialPlayerPositionX, this.initialPlayerPositionY);
    }

    private void SetPostionPlayers(int positionX, int positionY)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        int compteur = 0;
        foreach (GameObject player in players)
        {
            Debug.Log("sa passe frero");
            player.GetComponent<PlayerBoson>().UseSpawnPlayerRPC(positionX, positionY, compteur);
            compteur++;
        }
    }


    // Update is called once per frame
    void Update () {

    }

 
}
