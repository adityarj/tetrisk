using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpamBlockPowerup : PowerUpController {
	
	private PowerupMessage spamMessage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override Powerup getPowerup ()
	{
		return Powerup.SpamBlock;
	}

	//Detect powerup
	void OnTriggerStay2D(Collider2D other) {

		Debug.Log ("Spam block powerup method");
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("DeadBlock")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				spamMessage = new PowerupMessage ();
				spamMessage.x = other.transform.position.x;
				//NetworkServer.SendToAll (7997, spamMessage);
			}
		}
	}

	//Execute the powerup once the fortune wheel has finished spinning
	public override void executePowerup ()
	{
		NetworkServer.SendToAll (7997, spamMessage);
	}
		
}
