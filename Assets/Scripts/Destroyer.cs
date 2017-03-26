using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
	private Collider2D block;

	void OnTriggerEnter2D(Collider2D other) {
		this.block = other;
		// collider is square, so we need to check the tag of the parent object
		if (other.transform.parent.gameObject.tag == "falling") {
			other.GetComponentInParent<BlockController>().SetSpawnNext(true);
		}
		StartCoroutine(WaitAndDestroy(0.2f));
	}

	// Destroy parent block after given number of seconds
	IEnumerator WaitAndDestroy(float time) {
		yield return new WaitForSeconds(time);
		Destroy(this.block.transform.parent.gameObject);
	}
}
