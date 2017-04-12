using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBlockSlow : PowerUpController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {
		Debug.Log (playerTransform.position.x + " x " + other.x);
		return playerTransform.position.x + 3 > other.x && playerTransform.position.x - 3 < other.x;
	}

	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			base.setCollected (true);
		}
	}
}
