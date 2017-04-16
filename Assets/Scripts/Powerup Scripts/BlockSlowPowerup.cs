using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockSlowPowerup : PowerUpController {

	private PowerupMessage slowMessage;
	private NetworkClient client;

	public override Powerup getPowerup ()
	{
		return Powerup.BlockSlow;
	}

	public override void setClient (NetworkClient client)
	{
		this.client = client;
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
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;				
			}
		}
	}

	//Execute the powerup once the fortune wheel has finished spinning
	public override void executePowerup() {
		NetworkServer.SendToAll (7998, slowMessage);
		client.Send (7998, slowMessage);
	}
}
