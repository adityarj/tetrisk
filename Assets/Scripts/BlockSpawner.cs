using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
	
	public GameObject[] blockList;
	private int randIndex;

	public GameObject getBlock() {
		randIndex = Random.Range (0, blockList.Length);
		return blockList [randIndex];
	}

	public GameObject getSameBlock() {
		return blockList [randIndex];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
