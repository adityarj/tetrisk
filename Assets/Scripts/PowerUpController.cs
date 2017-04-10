using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpController : MonoBehaviour {

	private bool collected = false;

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
