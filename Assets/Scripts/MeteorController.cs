using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour {
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb.isKinematic =false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.CompareTag("powerMeteorActive")) {
			rb.isKinematic = true;
		}
	}
}
