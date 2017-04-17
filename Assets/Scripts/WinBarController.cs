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
		rb.velocity = new Vector3 (0, -0.3f, 0);

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

	void OnTriggerEnter2D(Collider2D other) {
		
		//Check if this function has already been called
		if (!this.getWin()) {

			GameObject parentBlock = other.transform.parent.gameObject;

			//If the parent is a deadblock, then end condition, sendd a message to stop the game
			if (parentBlock.CompareTag ("DeadBlock")) {
				setWin (true);
				Debug.Log ("Win! Game Over");

				EndGameMessage endgame = new EndGameMessage ();

				endgame.player = 0;
				endgame.message = "What a baller";
				endgame.x = other.transform.position.x;
				NetworkServer.SendToAll (7999, endgame);
			}
		}
	}
}
