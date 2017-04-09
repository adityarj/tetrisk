using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Rigidbody2D rb;
	private bool spawnSame = false;
	private bool spawnNext = false;

	[SerializeField]
	private GameObject winBar;

	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
		gameObject.tag = "falling";

		//We initially ignore the collision between the winBar gameObject and this collider
		Physics2D.IgnoreCollision (gameObject.GetComponent<Collider2D>(), winBar.GetComponent<Collider2D> ());
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
