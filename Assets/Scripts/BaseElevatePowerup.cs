using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseElevatePowerup : PowerUpController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool checkBounds(Transform playerTransform, Vector3 other) {

		return playerTransform.position.x + 3 > other.x && playerTransform.position.x - 3 < other.x;
	}

	void OnTriggerStay2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			base.setCollected (true);
			NetworkManager networkManager = NetworkManager.singleton;
			List<GameObject> players = networkManager.spawnPrefabs;

			foreach (GameObject player in players) {
				Debug.Log (player.transform.position);
				if (this.checkBounds (player.transform, new Vector3 (other.transform.position.x, other.transform.position.y, 0))) {
					Debug.Log ("works");
					GameObject baseBar = player.transform.Find("base").gameObject;
					GameObject basePlaceholder = player.transform.Find ("Base").gameObject;

					baseBar.transform.position += new Vector3 (0, 1, 0);
					basePlaceholder.transform.position += new Vector3 (0, 1, 0);
				}
			}

		}
	}
}
