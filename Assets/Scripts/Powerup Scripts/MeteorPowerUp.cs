using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeteorPowerUp : PowerUpController {

	[SerializeField]
	private GameObject Meteor;
	private Rigidbody2D rb;
	private GameObject actualMeteor;

	// Use this for initialization
	void Start () {
		
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {
		
		return playerTransform.position.x + 4 > other.x && playerTransform.position.x - 4 < other.x;
	}

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

	void OnTriggerEnter2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			base.setCollected (true);
			this.CmdSpawnMeteor (other.transform.position.x,other.transform.position.y);
			Destroy (gameObject);
		}
	}
}
