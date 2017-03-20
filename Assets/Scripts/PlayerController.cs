using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	private GameObject activeSquare;
	private BlockController activeBlockControl;
	private Vector3 spawnPosition;
	private RandomItemSpawner randomItemSpawner;
	private double[] bounds = new double[2];
	private float lastTime = 0;

	// Use this for initialization
	void Start () {
		
		//Start spawner
		spawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z) + new Vector3 (0, 12, 0);
		bounds [1] = spawnPosition.x + 3;
		bounds [0] = spawnPosition.x - 3;

		randomItemSpawner = FindObjectOfType<RandomItemSpawner> ();
		//If the script runs on a client, spawn for that client
		if (isLocalPlayer) {
			SpawnSquare ();
		}
	}

	//This function exists in case more code needs to be put in SpawnSquare()
	public void SpawnSquare() {
		CmdSpawnSquare ();
	}

	//Command to the server to spawn a square over the network
	[Command]
	public void CmdSpawnSquare() {
		activeSquare = Instantiate (randomItemSpawner.getBlock());
		activeSquare.transform.position = spawnPosition;
		NetworkServer.SpawnWithClientAuthority (activeSquare,gameObject);

		RpcSyncSpawnedObject (activeSquare);
		
	}

	//Client code to receive the instantiated object from the server
	[ClientRpc]
	public void RpcSyncSpawnedObject(GameObject squareRef) {
		activeSquare = squareRef;
		if (isLocalPlayer) {
			activeBlockControl = activeSquare.GetComponent<BlockController> ();
			Debug.Log (activeBlockControl.getVelocity ());
			activeBlockControl.setVelocity (new Vector3 (0, -1, 0));
		}
	}

	//Check if the left arrow key can exceed the bounderies of the game
	public bool checkValidBoundsLeft(Transform transform) {
		foreach (Transform child in transform) {
			if (child.position.x < bounds[0]) {
				return false;
			}
		}
		return true;
	}

	//Check if the right arrow key can exceed the boundaries of the game
	public bool checkValidBoundsRight(Transform transform) {
		foreach (Transform child in transform) {
			if (child.position.x >= bounds [1]) {
				return false;
			}
		}
		return true;
	}
	// Update is called once per frame
	void Update () {

		//Only transform the local objects, othewise ignore
		if (isLocalPlayer) {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (this.checkValidBoundsLeft(activeSquare.transform)) {
					activeSquare.transform.position += new Vector3 ((float)-0.5, 0, 0);
				}
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (this.checkValidBoundsRight(activeSquare.transform)) {
					activeSquare.transform.position += new Vector3 ((float)0.5, 0, 0);
				}
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				//Handle Up Arrow
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				activeSquare.transform.position+= new Vector3(0,(float)-0.5,0);
				lastTime = Time.time;
			}
		}
	}
}
