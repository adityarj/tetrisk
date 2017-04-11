﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Rigidbody2D rb;
	private bool spawnSame = false;
	private bool spawnNext = false;
	private bool hasRun = false;
	[SerializeField] private GameObject winBar;
	[SerializeField] private GameObject shadow;

	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
		gameObject.tag = "falling";
		shadow = Instantiate (shadow);
		shadow.transform.position = gameObject.transform.position;
	}

	void Update() {
		if (!gameObject.CompareTag ("Untagged")) {
			shadow.transform.position = gameObject.transform.position;
		} else {
			Destroy (shadow);
		}
	}

	private void setScaling() {
		foreach (Transform childTransform in gameObject.transform) {
			if (childTransform.position.x - 0.5 < shadow.transform.position.x - 0.5) {
				shadow.transform.localScale += new Vector3 (0.5f, 0, 0);
				shadow.transform.position += new Vector3 (-0.5f, 0, 0);
			} 

			if (childTransform.position.x + 0.5 > shadow.transform.position.x + 0.5) {
				shadow.transform.localScale += new Vector3 (0.5f, 0, 0);
			}
		}
	}
		
	public void SetSpawnNext(bool boolean) {
		this.spawnNext = boolean;
	}

	public bool GetSpawnNext() {
		return spawnNext;
	}

	public void setVelocity(Vector3 vel) {
		this.rb.velocity = vel;
	}

	public Vector3 getVelocity() {
		return rb.velocity;
	}

	void OnCollisionEnter2D(Collision2D collision) {
			
		if (collision.collider.CompareTag("wall")) {
			return;
		}
		if (gameObject.CompareTag("falling")) {

			//Once the object has been relegated to a plain block, we make it collidable again.
			Physics2D.IgnoreCollision (gameObject.GetComponent<Collider2D>(), winBar.GetComponent<Collider2D> (), false);
			gameObject.tag = "Untagged";
			SetSpawnNext(true);
		}
	}

	//Method called when downward arrow is pressed.
	public void applyDownwardForce() {
		this.rb.AddForce (new Vector3 (0, -900, 0));
	}
}
