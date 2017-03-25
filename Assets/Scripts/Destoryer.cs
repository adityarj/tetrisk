using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoryer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		// collider is square, so we need to check the tag of the parent object
		if (other.transform.parent.gameObject.tag == "falling") {
			FindObjectOfType<PlayerController>().SpawnBlock();
		}
		// destory parent(group)
		Destroy(other.transform.parent.gameObject);
	}
}
