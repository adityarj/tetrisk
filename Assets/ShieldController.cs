using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShieldController : MonoBehaviour {

	private bool setCountdown = false;

	void DestroyShield() {
		NetworkServer.Destroy (gameObject);
	}

	void OnTriggerEnter2d(Collider2D other) {
		if (!this.setCountdown) {
			if (other.gameObject.CompareTag ("Untagged")) {
				this.setCountdown = true;
				NetworkServer.Destroy (other.gameObject);
				Invoke ("DestroyShield", 4);
			}
		}
	}
}
