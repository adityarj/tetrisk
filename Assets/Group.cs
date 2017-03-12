using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("collsion");
		if (gameObject.CompareTag("falling")) {
			Debug.Log ("falling object");
			gameObject.tag = "Untagged";
			FindObjectOfType<Spawner>().spawnNext();
		}
	}
}
