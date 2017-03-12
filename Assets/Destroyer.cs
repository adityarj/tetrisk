using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Enter trigger region");
		Debug.Log(other.transform.parent.gameObject.tag);
		// collider is block, so we need to check the tag of the parent object
		if (other.transform.parent.gameObject.tag == "falling") {
			Debug.Log("trigger object is falling");
			FindObjectOfType<Spawner>().spawnSame();
		}
		// destory parent(group)
		Destroy(other.transform.parent.gameObject);
	}
}
