using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	[SerializeField]
	private GameObject square;
	private GameObject activeSquare;
	private Vector3 spawnPosition;
	private double[] bounds = new double[2];
	private float lastTime = 0;
	// Use this for initialization
	void Start () {
		//Start spawner
		spawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z) + new Vector3 (0, 12, 0);
		bounds [1] = spawnPosition.x + 2;
		bounds [0] = spawnPosition.x - 2;

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
		activeSquare = Instantiate (square);
		activeSquare.transform.position = spawnPosition;
		NetworkServer.SpawnWithClientAuthority (activeSquare,gameObject);
		RpcSyncSpawnedObject (activeSquare);
	}

	//Client code to receive the instantiated object from the server
	[ClientRpc]
	public void RpcSyncSpawnedObject(GameObject squareRef) {
		activeSquare = squareRef;
	}

	// Update is called once per frame
	void Update () {

		//Only transform the local objects, othewise ignoreh
		if (isLocalPlayer) {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (activeSquare.transform.position.x >= bounds [0]) {
					activeSquare.transform.position += new Vector3 ((float)-0.5, 0, 0);
				}
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (activeSquare.transform.position.x <= bounds [1]) {
					activeSquare.transform.position += new Vector3 ((float)0.5, 0, 0);
				}
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				//Handle Up Arrow
			} else if (Input.GetKeyDown (KeyCode.DownArrow) || Time.time - lastTime > 1) {
				activeSquare.transform.position+= new Vector3(0,(float)-0.5,0);
				lastTime = Time.time;
			}
		}
	}
}
