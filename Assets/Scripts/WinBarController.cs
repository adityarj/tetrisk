using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBarController : MonoBehaviour {

	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector3 (0, -0.5f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("falling")) {

			//When the collision happens, ignore it for each and every collider object
			foreach (Collider2D smallBlock in collision.gameObject.GetComponentsInChildren<Collider2D>()) {
				Physics2D.IgnoreCollision (smallBlock, gameObject.GetComponent<Collider2D> ());
			}
			
		} else {

			//When the collision happens, ignore it for each and every collider object
			foreach (Collider2D smallBlock in collision.gameObject.GetComponentsInChildren<Collider2D>()) {
				Physics2D.IgnoreCollision (smallBlock, gameObject.GetComponent<Collider2D> ());
			}
		}
	}
}
