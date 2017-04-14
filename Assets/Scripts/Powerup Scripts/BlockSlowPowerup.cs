using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockSlowPowerup : PowerUpController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override Powerup getPowerup ()
	{
		return Powerup.BlockSlow;
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {
		Debug.Log (playerTransform.position.x + " x " + other.x);
		return playerTransform.position.x + 3 > other.x && playerTransform.position.x - 3 < other.x;
	}

	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("DeadBlock")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				PowerupMessage slowMessage = new PowerupMessage ();
				slowMessage.x = other.transform.position.x;
				NetworkServer.SendToAll (7998, slowMessage);
			}
		}
	}
}
