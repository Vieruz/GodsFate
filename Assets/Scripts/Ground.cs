using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

    public int resourceGainedWhenPrayingIndex;

    public int[] resources;

    public GameObject[] groundPrefabs;

    const int AIR_INDEX = 0;
    const int FIRE_INDEX = 1;
    const int EARTH_INDEX = 2;
    const int WATER_INDEX = 3;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetResourceFromGround(int value)
    {
        if (value > resources[resourceGainedWhenPrayingIndex])
        {
            value = resources[resourceGainedWhenPrayingIndex];
        }
        resources[resourceGainedWhenPrayingIndex] -= value;
        int randomEvolution = Random.Range(0, resources.Length);
        resources[randomEvolution] += value;
        CheckGroundEvolution();
        return value;
    }

    void CheckGroundEvolution()
    {
        int groundTypeResources = resources[resourceGainedWhenPrayingIndex];
        for(int i = 0; i < resources.Length; i++)
        {
            if(resources[i] > groundTypeResources && i != resourceGainedWhenPrayingIndex)
            {
                Instantiate<GameObject>(groundPrefabs[i], transform.position, Quaternion.identity, transform.parent);
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Powers")
        {
            Power p = collision.gameObject.GetComponent<Power>();
            resources[p.resourceIndexGiven] += p.resourceGroundModifier;
            CheckGroundEvolution();
        }

    }
}
