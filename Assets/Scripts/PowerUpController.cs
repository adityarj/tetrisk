using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class PowerUpController : NetworkBehaviour {

	private bool collected = false;

	abstract public Powerup getPowerup ();

	public void setCollected(bool collected) {
		this.collected = collected;
	}

	public bool getCollected() {
		return this.collected;
	}

	public void DestoryPowerUp() {
		Destroy(gameObject);
	}
}
