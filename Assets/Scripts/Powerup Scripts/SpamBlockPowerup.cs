using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpamBlockPowerup : PowerUpController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Spam block powerup method");
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				PowerupMessage spamMessage = new PowerupMessage ();
				spamMessage.x = other.transform.position.x;
				NetworkServer.SendToAll (7997, spamMessage);
			}
		}
	}
}
