using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		Debug.Log ("BC Start() called");
		rb = this.GetComponent<Rigidbody2D> ();
		gameObject.tag = "falling";
		rb.velocity = new Vector3 (0, 0, 0);
	}

	public void setVelocity(Vector3 vel) {
		if (rb == null) {
			this.rb = GetComponent<Rigidbody2D> ();
			Debug.Log (rb);
		}
		rb.velocity = vel;
	}

	public Vector3 getVelocity() {
		return rb.velocity;
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.CompareTag("wall")) {
			return;
		}
		if (gameObject.CompareTag("falling")) {
			gameObject.tag = "Untagged";
			FindObjectOfType<PlayerController>().SpawnSquare();
		}
	}
}
