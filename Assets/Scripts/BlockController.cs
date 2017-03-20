using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		Debug.Log ("BC Start() called");
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector3 (0, 0, 0);
	}

	public void setVelocity(Vector3 vel) {
		rb.velocity = vel;
	}

	public Vector3 getVelocity() {
		return rb.velocity;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
