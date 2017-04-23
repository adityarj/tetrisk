using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpamBlockPowerup : PowerUpController {
	
	private PowerupMessage spamMessage;
	private NetworkClient client;
	private BlockSpawner blockSpawner;
	private float[] coord;

	void Start() {
		blockSpawner = FindObjectOfType<BlockSpawner> ();
	}

	public override void setClient (NetworkClient client) {
		this.client = client;
	}

	public override Powerup getPowerup ()
	{
		return Powerup.SpamBlock;
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {

		return playerTransform.position.x + 4 > other.x && playerTransform.position.x - 4 < other.x;
	}

	//Spawn blocks local to the respective player
	private void SpawnSpamBlocks(float x, float y) {

		NetworkManager networkManager = NetworkManager.singleton;
		List<Transform> playerPositions = networkManager.startPositions;

		foreach (Transform playerPosition in playerPositions) {
			if (!this.checkBounds (playerPosition, new Vector3(x,y,0))) {
				for (int i = 0; i < 5; i ++) {
					GameObject block = Instantiate (blockSpawner.getBlock());
					block.tag = "fallingrain";
					block.transform.position = playerPosition.position + new Vector3 (-2 + i, 14, 0);
					NetworkServer.Spawn (block);
				}
			}
		}
	}

	//Detect powerup
	void OnTriggerStay2D(Collider2D other) {

		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("DeadBlock")) {
			if (!base.getCollected()) {
				base.setCollected (true);
				spamMessage = new PowerupMessage ();
				spamMessage.x = other.transform.position.x;
				coord = new float[] { other.transform.position.x, other.transform.position.y };
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}

	//Execute the powerup once the fortune wheel has finished spinning
	public override void executePowerup () {
		SpawnSpamBlocks(coord[0], coord[1]);
	}
		
}
