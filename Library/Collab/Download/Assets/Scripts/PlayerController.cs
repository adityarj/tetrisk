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

		if (isLocalPlayer) {
			SpawnSquare ();
		}

	}

	public void SpawnSquare() {
		CmdSpawnSquare ();
	}

	[Command]
	public void CmdSpawnSquare() {
		activeSquare = Instantiate (square);
		activeSquare.transform.position = spawnPosition;
		NetworkServer.SpawnWithClientAuthority (activeSquare,gameObject);
		RpcSyncSpawnedObject (activeSquare);
	}

	[ClientRpc]
	public void RpcSyncSpawnedObject(GameObject squareRef) {
		activeSquare = squareRef;
	}
	// Update is called once per frame
	void Update () {

		//Only transform the local objects, othewise ignore
		if (isLocalPlayer) {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				//Handle left arrow
				if (activeSquare.transform.position.x >= bounds [0]) {
					activeSquare.transform.position += new Vector3 ((float)-0.5, 0, 0);
				}
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				//Handle Right arrow
				if (activeSquare.transform.position.x <= bounds [1]) {
					activeSquare.transform.position += new Vector3 ((float)0.5, 0, 0);
				}
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				//Handle Up Arrow
				//activeSquare.transform.position+= new Vector3(0,1,0);
			} else if (Input.GetKeyDown (KeyCode.DownArrow) || Time.time - lastTime > 1) {
				//Handle down arrow
				activeSquare.transform.position+= new Vector3(0,(float)-0.5,0);
				lastTime = Time.time;
			}
		}
	}
}
