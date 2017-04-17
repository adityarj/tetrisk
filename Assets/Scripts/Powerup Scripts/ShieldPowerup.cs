using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShieldPowerup : PowerUpController {

	private PowerupMessage shieldMessage;
	private NetworkClient client;

	public override void setClient(NetworkClient client) {
		this.client = client;
	}

	public override Powerup getPowerup() {
		return Powerup.Shield;
	}

	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag ("DeadBlock")) {
			if (!base.getCollected ()) {
				base.setCollected (true);
				shieldMessage = new PowerupMessage ();
				shieldMessage.x = other.transform.position.x;
]				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}

	public override void executePowerup ()
	{
		NetworkServer.SendToAll (7996, shieldMessage);
		client.Send (7996, shieldMessage);
	}
}
