using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Game : MonoBehaviourPun
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

    public int SCALE_X = 34;
    public int SCALE_Y = -17;
    public float INITIATE_POSITION = 10;
    public int SPACE_SCALE = 3;

    private int initialPlayerPositionX;
    private int initialPlayerPositionY;

    public GameObject parent;

    // Use this for initialization
    void Start () {

        StartRound();
        Camera.main.aspect = 2960f / 1440f;
    }

 
    public void StartRound()
    {
        while (InitateMap(widthMap, heightMap, randomDelete) < minimumRoom) ;
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayers();
            InitiateExit();
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
                mapInstantiate.transform.parent = parent.transform;
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

    void SpawnZombie(int numberZombie)
    {
        int positionX;
        int positionY;
        int positionXPlayer;
        int positionYPlayer;
        for (int i = 0; i < numberZombie; i++)
        {

            do
            {
                GameObject randomPlayer = GameObject.Find("Perso(Clone)");
                positionXPlayer = randomPlayer.GetComponent<PlayerBoson>().positionX;
                positionYPlayer = randomPlayer.GetComponent<PlayerBoson>().positionY;

                positionX = Random.Range(1, widthMap);
                positionY = Random.Range(1, heightMap);

            } while (positionX == positionXPlayer && positionY == positionYPlayer);

            GameObject room = GameObject.Find("Room_" + positionX + "_" + positionY);
            int counter = room.GetComponent<AddZombie>().counter;

            float[] precisePostion = SpawnZombieAtPrecisePosition(counter);

            float shiftX = precisePostion[0];
            float shiftY = precisePostion[1];

            photonView.RPC("InstantiateZombie", RpcTarget.All, positionX, positionY, shiftX, shiftY);

        }

    }

    [PunRPC]
    void InstantiateZombie(int positionX, int positionY, float shiftX, float shiftY)
    {
        Transform zombieInstantiate = Instantiate(Resources.Load<Transform>("Zombie"), new Vector3(shiftX + (positionX * SCALE_X), shiftY + (positionY * SCALE_Y), 1), Quaternion.identity);
        zombieInstantiate.parent = parent.transform;
        Zombie movingScript = zombieInstantiate.GetComponent<Zombie>();
        movingScript.Xposition = positionX;
        movingScript.Yposition = positionY;

        GameObject room = GameObject.Find("Room_" + positionX + "_" + positionY);
        int counter = room.GetComponent<AddZombie>().counter;
        room.GetComponent<AddZombie>().counter = room.GetComponent<AddZombie>().counter + 1;
    }

    private float[] SpawnZombieAtPrecisePosition(int counter)
    {
        float[] precisePosition = { 0, 0 };

        if (counter == 0)
        {
            precisePosition[0] = -13;
            precisePosition[1] = 5.26f;

            return precisePosition;
        }
        if (counter == 1)
        {
            precisePosition[0] = 13;
            precisePosition[1] = 5.26f;
            return precisePosition;
        }
        if (counter == 2)
        {
            precisePosition[0] = 13;
            precisePosition[1] = -5.26f;
            return precisePosition;
        }
        if (counter == 3)
        {
            precisePosition[0] = -13;
            precisePosition[1] = -5.26f;
            return precisePosition;
        }
        return precisePosition;
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

        int counter = 0;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerBoson>().UseSpawnPlayerRPC(positionX, positionY, INITIATE_POSITION, counter, SPACE_SCALE);
            counter++;
        }
    }


    public void CheckSurvivor()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i =0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<PlayerBoson>().won)
            {
                return;
            }
        }
        photonView.RPC("EndGame", RpcTarget.All);
        //EndGame();
    }

    [PunRPC]
    private void EndGame()
    {
        Destroy(GameObject.Find("Map"));
        GameObject newMap = new GameObject("Map");
        parent = newMap;
        ChangeStatePlayer();
        StartRound();
    }

    private void ChangeStatePlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            GameObject player = players[i];
            player.GetComponent<PlayerBoson>().UseSetStateRPC(6, false);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }


}
