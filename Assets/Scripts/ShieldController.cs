using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShieldController : MonoBehaviour {

	private bool setCountdown = false;
	private float transformY = 0;
	private Transform startTransform;
	private bool raiseFlag = false;

	void Start() {
		this.startTransform = gameObject.transform;
	}

	void DestroyShield() {
		Debug.Log ("Destroy triggered");
		NetworkServer.Destroy (gameObject);
	}

	void FixedUpdate() {
		
		if (!raiseFlag) {
			
			gameObject.transform.position += new Vector3(0,0.2f * Time.deltaTime,0) ;
			this.transformY += 0.2f * Time.deltaTime;

			if (this.startTransform.position.y + this.transformY > 1) {
				this.raiseFlag = true;
				this.transformY = 0;
			}
		} else {
			
			gameObject.transform.position -= new Vector3(0,0.2f * Time.deltaTime,0);
			this.transformY -= 0.2f * Time.deltaTime;

			if (this.startTransform.position.y + this.transformY < -1) {
				this.raiseFlag = false;
				this.transformY = 0;
			}
		}
	}
			

	void OnTriggerEnter2D(Collider2D other) {
		
		if (!other.gameObject.CompareTag ("falling") || !other.gameObject.CompareTag("winBar")) {

			NetworkServer.Destroy (other.gameObject);

			if (!this.setCountdown) {
				Invoke ("DestroyShield", 15);
				this.setCountdown = true;
			}
		}
		
	}
}
