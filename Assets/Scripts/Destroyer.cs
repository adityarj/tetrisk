using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
	private GameObject parentBlock;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("powerMeteor")) {
			other.gameObject.tag = "powerMeteorActive";
			return;
		} else if (other.gameObject.CompareTag("powerMeteorActive")) {
			Destroy(other.gameObject);
		}
		this.parentBlock = other.transform.parent.gameObject;
		// collider is square, so we need to check the tag of the parent object
		if (other.transform.parent.gameObject.tag == "falling") {
			other.GetComponentInParent<BlockController>().SetSpawnNext(true);
		}
		StartCoroutine(WaitAndDestroy(0.2f));
	}

	// Destroy parent block after given number of seconds
	IEnumerator WaitAndDestroy(float time) {
		while (this.parentBlock != null) {
			yield return new WaitForSeconds(time);
			Destroy(this.parentBlock);
		}
	}
}
