using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseElevatePowerup : PowerUpController {

	private NetworkClient client;

	public override void setClient (NetworkClient client) {
		this.client = client;
	}

	public override Powerup getPowerup ()
	{
		return Powerup.BaseElevate;
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {
		Debug.Log (playerTransform.position.x + " x " + other.x);
		return playerTransform.position.x + 3 > other.x && playerTransform.position.x - 3 < other.x;
	}

	//Detect powerup
	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("DeadBlock")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}

	//Execute the powerup once the fortune wheel has finished spinning
	public override void executePowerup ()
	{
		Debug.Log ("WHY MAI LIFE LIKE DIS");
	}
}
