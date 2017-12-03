using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int houseSpawnChance = 1;

    public GameObject[] groundPrefabs;
    public GameObject housePrefabs;
    public TextAsset mapFile;
    public int width;

    const int HOUSE_TYPE = -1;
    const int WATER_TYPE = 1;
    public const int SAND_TYPE = 2;
    public const int PLAIN_TYPE = 3;
    public const int FOREST_TYPE = 4;
    public const int MOUNTAIN_TYPE = 5;

	// Use this for initialization
	void Start () {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateMap()
    {
        string map = mapFile.text;
        int i = 0;
        int heigth = 0;
        int previousGround;
        for(int y = 0; i < map.Length; y--)
        {
            for(int x = 0; x < width; x++)
            {
                if(i < map.Length)
                {
                    //Build map depending on values in mapFile.
                    int groundType;
                    int.TryParse(map[i].ToString(), out groundType);
                    if (groundType > 0)
                    {
                        previousGround = groundType;
                        Instantiate<GameObject>(groundPrefabs[groundType - 1], new Vector3(x, y, 0), Quaternion.identity, transform);
                        //If Plain, maybe there is an house.
                        //but not if previous ground was a water or an house.
                        if(groundType == 3 && previousGround != 1 && previousGround != -1)
                        {
                            int r = Random.Range(0, 100);
                            if(r <= houseSpawnChance)
                            {
                                previousGround = -1;
                                Instantiate<GameObject>(housePrefabs, new Vector3(x, y, 0), Quaternion.identity, transform);
                            }
                        }
                    }
                    else
                    {
                        //Case of value that are not ground.
                        x--;
                    }
                    i++;
                }
            }
            heigth = y;
        }

        //Center board position
        transform.position = new Vector3(-width / 2, -heigth / 2, 0);
    }
}
