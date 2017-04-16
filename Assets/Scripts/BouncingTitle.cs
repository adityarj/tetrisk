using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingTitle : MonoBehaviour {

	public bool move = true;
	public Vector3 MoveVector = Vector3.up;
	public float MoveRange = 0.1f;
	public float MoveSpeed = 0.5f;

	private BouncingTitle bouncingTitle;

	Vector3 startPosition;

	// Use this for initialization
	void Start () {
		bouncingTitle = this;
		startPosition = bouncingTitle.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (move) {
			bouncingTitle.transform.position = startPosition + MoveVector * (MoveRange * Mathf.Sin (Time.timeSinceLevelLoad * MoveSpeed));
		}
	}
}
