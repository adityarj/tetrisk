﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeteorPowerUp : PowerUpController {

	[SerializeField]
	private GameObject Meteor;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {
		
		return playerTransform.position.x + 3 > other.x && playerTransform.position.x - 3 < other.x;
	}

	[Command]
	private void CmdSpawnMeteor(float x, float y) {
		Meteor = Instantiate (Meteor);
		NetworkManager networkManager = NetworkManager.singleton;
		List<Transform> playerPositions = networkManager.startPositions;

		foreach (Transform playerPosition in playerPositions) {
			if (checkBounds (playerPosition, new Vector3(x,y,0))) {
				Meteor.transform.position = new Vector3 (x - 10, y + 8, 0);
				rb = Meteor.GetComponent<Rigidbody2D> ();
				rb.velocity = new Vector3 ( (float)4, -4, 0);
				NetworkServer.Spawn (Meteor);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			base.setCollected (true);
			this.CmdSpawnMeteor (other.transform.position.x,other.transform.position.y);
		}
	}
}
