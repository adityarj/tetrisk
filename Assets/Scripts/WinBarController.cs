using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBarController : MonoBehaviour {
	private Rigidbody2D rb;
	private bool win;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector3 (0, -0.5f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setWin(bool win) {
		this.win = win;
	}

	public bool getWin() {
		return this.win;
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			setWin(true);
			Debug.Log("WINNSAFS");
		}
	}
}
