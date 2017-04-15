using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockSlowPowerup : PowerUpController {

	private PowerupMessage slowMessage;

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

	private bool checkBounds(Transform playerTransform, Vector3 other) {
		Debug.Log (playerTransform.position.x + " x " + other.x);
		return playerTransform.position.x + 3 > other.x && playerTransform.position.x - 3 < other.x;
	}

	//Detect powerup
	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("DeadBlock")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				slowMessage = new PowerupMessage ();
				slowMessage.x = other.transform.position.x;
				//NetworkServer.SendToAll (7998, slowMessage);
			}
		}
	}

	//Execute the powerup once the fortune wheel has finished spinning
	public override void executePowerup() {
		NetworkServer.SendToAll (7998, slowMessage);
	
	}
}
