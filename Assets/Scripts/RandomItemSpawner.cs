using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour {
	
	public GameObject[] blockList;

	public GameObject getBlock() {
		int randIndex = Random.Range (0, blockList.Length);
		return blockList [randIndex];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
