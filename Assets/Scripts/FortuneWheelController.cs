using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneWheelController : MonoBehaviour {

	private PowerupState state;
	private int angle = 0;
	private int iterAngle = 0;
	private int iterWait = 0;
	// Use this for initialization
	void Start () {
		this.state = PowerupState.NoPowerup;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.state.Equals(PowerupState.NoPowerup)) {
			return;
		} else if (this.state.Equals(PowerupState.ActivePowerup)) {
			//Handle rotation to designated coordinates
			if (iterAngle > this.angle) {
				this.state = PowerupState.Waiting;
				this.iterAngle = 0;
			} else {
				gameObject.transform.Rotate (new Vector3 (0, 0, 180 * Time.deltaTime));
				this.iterAngle += 1;
			}
		} else if (this.state.Equals(PowerupState.Waiting)) {
			if (this.iterWait > 100) {
				this.state = PowerupState.Refreshing;
				this.iterWait = 0;
			} else {
				this.iterWait += 1;
			}
		} else if (this.state.Equals(PowerupState.Refreshing)) {
			//Handle restored state
			if (iterAngle > this.angle) {
				this.state = PowerupState.NoPowerup;
				this.angle = 0;
				this.iterAngle = 0;
			} else {
				gameObject.transform.Rotate (new Vector3 (0, 0, -180 * Time.deltaTime));
				this.iterAngle += 1;
			}
		}
	}

	//Do something based on each value
	public void HandlePowerup(Powerup powerup) {
		
		if (powerup.Equals(Powerup.Meteor)) {
			//Rotate to meteor
			this.angle = 90;
		} else if (powerup.Equals(Powerup.BlockSlow)) {
			//Rotate to Block SLow
			this.angle = 90;
		} else if (powerup.Equals(Powerup.SpamBlock)) {
			//Rotate to spam block
			this.angle = 90;
		} else if (powerup.Equals(Powerup.BaseElevate)) {
			//Rotate to base elevate
			this.angle = 90;
		}

		this.state = PowerupState.ActivePowerup;
	}	
}

enum PowerupState {
	NoPowerup,
	ActivePowerup,
	Waiting,
	Refreshing
}