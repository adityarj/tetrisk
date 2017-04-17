using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShieldController : MonoBehaviour {

	private bool setCountdown = false;

	void DestroyShield() {
		NetworkServer.Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		Debug.Log ("<<Enter collision shield>>");
		if (!other.gameObject.CompareTag ("falling")) {
			
			NetworkServer.Destroy (other.gameObject);

			if (!this.setCountdown) {
				Invoke ("DestroyShield", 4);
				this.setCountdown = true;
			}
		}
		
	}
}
