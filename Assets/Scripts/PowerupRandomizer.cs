using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupRandomizer : PowerUpController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void CmdSpawnRandomizer(){
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameObject parentBlock = other.transform.parent.gameObject;
		if (parentBlock.CompareTag("Untagged")) {
			base.setCollected (true);
			this.CmdSpawnRandomizer ();
			Destroy (gameObject);
		}
	}
}
