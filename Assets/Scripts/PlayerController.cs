using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	//Related to blocks
	private GameObject activeBlock;
	private BlockController activeBlockControl;
	private Vector3 spawnPosition;
	private BlockSpawner blockSpawner;
	private double[] bounds = new double[2];
	private Touch initialTouch;

	//Related to Winning Bar
	[SerializeField]
	private GameObject winBar;
	private GameObject localWinBar;
	// Use this for initialization
	void Start () {
		
		//Start spawner
		spawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z) + new Vector3 (0, 12, 0);
		bounds [1] = spawnPosition.x + 3;
		bounds [0] = spawnPosition.x - 3;

		blockSpawner = FindObjectOfType<BlockSpawner> ();
		//If the script runs on a client, spawn for that client
		if (isLocalPlayer) {
			SpawnBlock ();
		}
	}

	//Called when the server is started
	public override void OnStartServer() {
		if (isServer && !WinBarSingleton.peekInstance()) {
			this.CmdSpawnWinBar ();
		}
	}

	//This function exists in case more code needs to be put in SpawnBlock()
	public void SpawnBlock() {
		CmdSpawnBlock ();
	}

	public void SpawnSameBlock() {
		CmdSpawnBlock();
	}

	//Command to the server to spawn a block over the network
	[Command]
	public void CmdSpawnBlock() {
		activeBlock = Instantiate(blockSpawner.getBlock());
		activeBlock.transform.position = spawnPosition;
		NetworkServer.SpawnWithClientAuthority (activeBlock,gameObject);
		RpcSyncSpawnedObject (activeBlock);
		
	}

	//Client code to receive the instantiated object from the server
	[ClientRpc]
	public void RpcSyncSpawnedObject(GameObject blockRef) {
		activeBlock = blockRef;
		activeBlockControl = activeBlock.GetComponent<BlockController> ();
	}

	//Spawn with the Win Bar on start.
	[Command]
	public void CmdSpawnWinBar() {
		this.localWinBar = Instantiate (WinBarSingleton.getInstance(winBar));
		this.localWinBar.transform.position = new Vector3 (5, 14, 0);
		Debug.Log (this.localWinBar);
		NetworkServer.Spawn (this.localWinBar);
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
			if (activeBlockControl != null) {
				activeBlockControl.setVelocity (new Vector3 (0, -1, 0));

				if (this.checkValidBoundsLeft(activeBlock.transform)) {
					if (Input.GetKeyDown(KeyCode.LeftArrow)) {
						activeBlock.transform.position += new Vector3((float)-0.5, 0, 0);
					}	
				}

				if (this.checkValidBoundsRight(activeBlock.transform)) {
					if (Input.GetKeyDown(KeyCode.RightArrow)) {
						activeBlock.transform.position += new Vector3((float)0.5, 0, 0);
					}
				}

				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					activeBlock.transform.Rotate(0,0,-90);
					if (!(this.checkValidBoundsLeft(activeBlock.transform) && this.checkValidBoundsRight(activeBlock.transform))){
						activeBlock.transform.Rotate(0,0,90);
					}
				}

				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					activeBlock.transform.position += new Vector3 (0,(float) -0.5, 0);
				}

				//Spawn block if necessary based on the status player
				if (activeBlockControl.GetSpawnNext()) {
					activeBlockControl = null;
					this.SpawnBlock ();
				}

				if (Input.touchCount > 0) {

					foreach (Touch t in Input.touches) {

						// handle rotation
						if (t.phase == TouchPhase.Began) {
							initialTouch = t;

							if (this.checkValidBoundsLeft (activeBlock.transform) && this.checkValidBoundsRight (activeBlock.transform)) {
								activeBlock.transform.Rotate (0, 0, -90);
							}

						// handle lateral movement
						} else if (t.phase == TouchPhase.Moved) {
							// user swiped left
							if (t.position.x - initialTouch.position.x < 0) {
								if (this.checkValidBoundsLeft (activeBlock.transform)) {
									activeBlock.transform.position += new Vector3 ((float)-0.5, 0, 0);
								}

							// user swiped right
							} else if (t.position.x - initialTouch.position.x > 0) {
								if (this.checkValidBoundsRight (activeBlock.transform)) {
									activeBlock.transform.position += new Vector3 ((float)0.5, 0, 0);
								}

							// user swiped up
							} else if (t.position.y - initialTouch.position.y > 0) {
								// handle up?

							// user swiped down
							} else if (t.position.y - initialTouch.position.y < 0) {
								activeBlock.transform.position += new Vector3 (0, (float)-0.5, 0);
							}
						}
					}
				}
			}
		}
	}
}
