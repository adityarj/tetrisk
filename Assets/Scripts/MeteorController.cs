using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour {
	private Rigidbody2D rb;
	private float rotationsPerMinute = 30.0f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.isKinematic =false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.CompareTag("powerMeteorActive")) {
			rb.isKinematic = true;
		}

		gameObject.transform.Rotate( 0,0,6.0f * Time.deltaTime * this.rotationsPerMinute);
	}
}
