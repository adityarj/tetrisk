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
		
		if (other.gameObject.CompareTag ("DeadBlock") || other.gameObject.CompareTag ("powerMeteor")) {
			
			this.setCountdown = true;
			NetworkServer.Destroy (other.gameObject);

			if (!this.setCountdown) {
				Invoke ("DestroyShield", 4);
			}
		}
		
	}
}
