using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneWheelController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Do something based on each value
	public void HandlePowerup(Powerup powerup) {
		
		if (powerup.Equals(Powerup.Meteor)) {
			//Rotate to meteor
			gameObject.transform.Rotate(new Vector3(0,0,90));
		} else if (powerup.Equals(Powerup.BlockSlow)) {
			//Rotate to Block SLow
			gameObject.transform.Rotate(new Vector3(0,0,90));
		} else if (powerup.Equals(Powerup.SpamBlock)) {
			//Rotate to spam block
			gameObject.transform.Rotate(new Vector3(0,0,90));
		} else if (powerup.Equals(Powerup.BaseElevate)) {
			//Rotate to base elevate
			gameObject.transform.Rotate(new Vector3(0,0,90));

		}
		
	}

	public void ResetFortuneWheel() {
		gameObject.transform.Rotate (new Vector3 (0, 0, 0));
	}

}