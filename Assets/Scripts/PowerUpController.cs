using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class PowerUpController : NetworkBehaviour {

	private bool collected = false;
	private bool moveBaseUp = false;
	private bool executed = false;

	abstract public void setClient (NetworkClient client);

	abstract public Powerup getPowerup ();

	abstract public void executePowerup ();

	public void setCollected(bool collected) {
		this.collected = collected;
	}

	public bool getCollected() {
		return this.collected;
	}

	public void setMoveBaseUp(bool moveBaseUp) {
		this.moveBaseUp = moveBaseUp;
	}

	public bool getMoveBaseUp() {
		return this.moveBaseUp;
	}

	public void setExecuted(bool executed) {
		this.executed = executed;
	}

	public bool getExecuted() {
		return this.executed;
	}

	public void DestoryPowerUp() {
		WaitAndDestroy(0.2f);
	}

	IEnumerator WaitAndDestroy(float time) {
		while (gameObject != null) {
			yield return new WaitForSeconds(time);
			Destroy(gameObject);
		}
	}
}
