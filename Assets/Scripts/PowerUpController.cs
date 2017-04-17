using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class PowerUpController : NetworkBehaviour {

	private bool collected = false;

	abstract public void setClient (NetworkClient client);

	abstract public Powerup getPowerup ();

	abstract public void executePowerup ();

	public void setCollected(bool collected) {
		this.collected = collected;
	}

	public bool getCollected() {
		return this.collected;
	}

	public void DestoryPowerUp() {
		NetworkServer.Destroy (gameObject);
	}
}
