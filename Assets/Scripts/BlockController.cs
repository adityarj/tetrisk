using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Rigidbody2D rb;
	private bool spawnSame = false;
	private bool spawnNext = false;

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
			shadow.transform.position = gameObject.transform.position + new Vector3(0,0,-1);
			Debug.Log (gameObject.GetComponent<Renderer> ().bounds.size);
		} else {
			Destroy (shadow);
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
