using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPowerUp : PowerUpController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			base.setCollected (true);
		}
	}
}
