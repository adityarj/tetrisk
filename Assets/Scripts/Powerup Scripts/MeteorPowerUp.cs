using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeteorPowerUp : PowerUpController {

	[SerializeField]
	private GameObject Meteor;
	private Rigidbody2D rb;
	private GameObject actualMeteor;
	private NetworkClient client;
	private float[] coord;

	public override void setClient (NetworkClient client) {
		this.client = client;
	}
		
	public bool checkBounds(Transform playerTransform, Vector3 other) {
		
		return playerTransform.position.x + 4 > other.x && playerTransform.position.x - 4 < other.x;
	}

	public override Powerup getPowerup ()
	{
		return Powerup.Meteor;
	}

	//Spawn the meteor at all positions that are players, but not the triggered player
	[Command]
	private void CmdSpawnMeteor(float x, float y) {
		
		NetworkManager networkManager = NetworkManager.singleton;
		List<Transform> playerPositions = networkManager.startPositions;

		foreach (Transform playerPosition in playerPositions) {
			if (!this.checkBounds (playerPosition, new Vector3(x,y,0))) {
				Debug.Log (playerPosition.position);
				actualMeteor = Instantiate (Meteor);

				if (playerPosition.position.x > 3) {
					actualMeteor.transform.position = new Vector3 (playerPosition.position.x + 14, playerPosition.position.y + 16, 0);
					rb = actualMeteor.GetComponent<Rigidbody2D> ();
					rb.velocity = new Vector3 ((float)-6, -4, 0);
				} else {
					actualMeteor.transform.position = new Vector3 (playerPosition.position.x - 14, playerPosition.position.y + 16, 0);
					rb = actualMeteor.GetComponent<Rigidbody2D> ();
					rb.velocity = new Vector3 ((float)6, -4, 0);
				}

				NetworkServer.Spawn (actualMeteor);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	//Detect powerup
	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("DeadBlock")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				coord = new float[] { other.transform.position.x, other.transform.position.y };
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}

	//Execute the powerup once the fortune wheel has finished spinning
	public override void executePowerup() {
		this.CmdSpawnMeteor (this.coord[0],this.coord[1]);
	}
		
}
