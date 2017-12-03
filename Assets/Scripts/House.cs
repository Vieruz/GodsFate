using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public GameObject humanPrefab;

	// Use this for initialization
	void Start () {
        SpawnHuman();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnHuman()
    {
        Instantiate<GameObject>(humanPrefab, new Vector3(transform.position.x - 1, transform.position.y, 0), Quaternion.identity, transform);
    }
}
