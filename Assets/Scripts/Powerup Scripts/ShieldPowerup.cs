using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShieldPowerup : PowerUpController {

	[SerializeField]
	private GameObject shield;
	private NetworkClient client;
	private float[] coord;

	public override void setClient(NetworkClient client) {
		this.client = client;
	}

	public override Powerup getPowerup() {
		return Powerup.Shield;
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {

		return playerTransform.position.x + 4 > other.x && playerTransform.position.x - 4 < other.x;
	}

	//Spawn the shield local to the respective player
	[Command]
	private void CmdSpawnShield(float x, float y) {

		NetworkManager networkManager = NetworkManager.singleton;
		List<Transform> playerPositions = networkManager.startPositions;

		foreach (Transform playerPosition in playerPositions) {
			if (this.checkBounds (playerPosition, new Vector3(x,y,0))) {
				Debug.Log (playerPosition);
				shield = Instantiate (shield);
				shield.transform.position = playerPosition + new Vector3 (0, 8, 0);
				NetworkServer.Spawn (shield);
			}
		}
	}

	//Detect powerup
	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag ("DeadBlock")) {
			if (!base.getCollected ()) {
				base.setCollected (true);
				coord = new float[] { other.transform.position.x, other.transform.position.y };
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}

	//Execute powerup
	public override void executePowerup ()
	{
		this.CmdSpawnShield (coord[0],coord[1]);
	}
}
