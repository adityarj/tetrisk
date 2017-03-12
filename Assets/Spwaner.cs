using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spwaner : MonoBehaviour {

	public GameObject[] groups;

	// Use this for initialization
	void Start () {
		spawnNext();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void spawnNext() {
		int i = Random.Range (0, groups.Length);

		// Spawn Group at current position
		Instantiate(groups[i], transform.position, Quaternion.identity);
	}
}
