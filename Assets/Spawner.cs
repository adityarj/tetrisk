using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] groups;
	private int i;

	// Use this for initialization
	void Start() {
		spawnNext();
	}

	// Update is called once per frame
	void Update() {

	}

	public void spawnNext() {
		Debug.Log("Creating next item");
		i = Random.Range(0, groups.Length);

		// Spawn Group at current position
		Instantiate(groups[i], transform.position, Quaternion.identity);
	}

	public void spawnSame(){
		Debug.Log("Creating same item");
		Instantiate(groups[i], transform.position, Quaternion.identity);
	}
}
