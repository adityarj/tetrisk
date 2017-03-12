using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.CompareTag("falling")) {
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				transform.position += new Vector3(-1, 0, 0);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				transform.position += new Vector3(1, 0, 0);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				transform.Rotate(0,0,-90);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.CompareTag("wall")) {
			return;
		}
		if (gameObject.CompareTag("falling")) {
			gameObject.tag = "Untagged";
			FindObjectOfType<Spawner>().spawnNext();
		}
	}
		
}
