using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpamBlockPowerup : PowerUpController {
	
	private PowerupMessage spamMessage;
	private NetworkClient client;

	public override void setClient (NetworkClient client) {
		this.client = client;
	}

	public override Powerup getPowerup ()
	{
		return Powerup.SpamBlock;
	}

	//Detect powerup
	void OnTriggerStay2D(Collider2D other) {

		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("DeadBlock")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				spamMessage = new PowerupMessage ();
				spamMessage.x = other.transform.position.x;
				//NetworkServer.SendToAll (7997, spamMessage);
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}

	//Execute the powerup once the fortune wheel has finished spinning
	public override void executePowerup () {
		NetworkServer.SendToAll (7997, spamMessage);
		client.Send (7997, spamMessage);
	}
		
}
