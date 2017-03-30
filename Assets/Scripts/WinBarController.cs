using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WinBarController : NetworkBehaviour {
	private Rigidbody2D rb;
	private bool win;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector3 (0, -0.05f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setWin(bool win) {
		this.win = win;
	}

	public bool getWin() {
		return this.win;
	}

	public bool checkBounds(Transform playerTransform, Transform blockTransform) {
		return playerTransform.position.x + 3 > blockTransform.position.x && playerTransform.position.x - 3 < blockTransform.position.x;
	}

	//Structure to send terminate game message
	public struct EndMessage {
		public string finalWords;
	}

	[Command]
	void CmdGameOver() {
		NetworkManager.Shutdown ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			setWin(true);
			Debug.Log("Win! Game Over");
			NetworkManager networkManager = NetworkManager.singleton;
			List<Transform> playerPositions = networkManager.startPositions;

			int i = 1;
			foreach (Transform playerPosition in playerPositions) {
				if (checkBounds(playerPosition, parentBlock.transform)) {

					EndMessage endMessage;
					endMessage.finalWords = "Player " + i + " won";
					Debug.Log(endMessage.finalWords);
					//NetworkServer.SendToAll ((short) 500,endMessage);
				}
			}
			CmdGameOver ();
		}
	}
}
