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
	private int activePowerUpCount = 0;
//	Camera playerCam;

//	void Awake() {
//		playerCam = GetComponentInChildren<Camera> ();
//		playerCam.gameObject.SetActive (false);
//	}
	//Related to Winning Bar
	[SerializeField]
	private GameObject winBar;
	private GameObject localWinBar;

	//Related to PowerUps
	private GameObject powerUp;
	private PowerUpController powerUpControl;
	private PowerUpSpawner powerUpSpawner;
	private bool powerUpPresent;

	// Use this for initialization
	void Start () {
		
		//Start spawner
		spawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z) + new Vector3 (0, 12, 0);
		bounds [1] = spawnPosition.x + 3;
		bounds [0] = spawnPosition.x - 3;


		blockSpawner = FindObjectOfType<BlockSpawner> ();
		powerUpSpawner = FindObjectOfType<PowerUpSpawner> ();
		//If the script runs on a client, spawn for that client
		if (isLocalPlayer) {
			SpawnBlock ();
			SpawnPowerUp();
		}

	}

	private void SpawnPowerUp() {
		StartCoroutine(WaitAndSpawnPowerUp(5f));
	}

	IEnumerator WaitAndSpawnPowerUp(float time) {
		yield return new WaitForSeconds(time);
		if (!powerUpPresent){
			powerUpPresent = true;
			CmdSpawnPowerUp();
		}
		SpawnPowerUp();
	}

	//Command to the server to spawn a power-up over the network
	[Command]
	public void CmdSpawnPowerUp() {
		powerUp = Instantiate(powerUpSpawner.getPowerUp());
		// TODO make position random
		powerUp.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z) + new Vector3 (2, 6, 0);
		NetworkServer.SpawnWithClientAuthority (powerUp,gameObject);
		RpcSyncSpawnedPowerUp (powerUp);
	}

	[ClientRpc]
	public void RpcSyncSpawnedPowerUp(GameObject powerUpRef) {
		powerUp = powerUpRef;
		powerUpControl = powerUp.GetComponent<PowerUpController> ();
	}

	//Called when the server is started
	public override void OnStartServer() {
		if (isServer && !WinBarSingleton.peekInstance()) {
			this.CmdSpawnWinBar ();
		}
	}

//	public override void OnStartLocalPlayer ()
//	{
//		playerCam.gameObject.SetActive (true);
//	}
	public override void OnStartClient ()
	{
		base.OnStartClient ();
		NetworkManager.singleton.client.RegisterHandler (7999, OnReceiveEndGameMessage);
		NetworkManager.singleton.client.RegisterHandler (7998, OnReceiveSlowMessage);
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

	//When a message is received to end the game
	public void OnReceiveEndGameMessage(NetworkMessage networkMessage) {
		EndGameMessage endgame = networkMessage.ReadMessage<EndGameMessage> ();
		Debug.Log ("Player " + endgame.player + " won");
	}

	//When a message is received to apply the slow powerup
	public void OnReceiveSlowMessage(NetworkMessage networkMessage) {
		Debug.Log ("Slow powerup");
		SlowPowerupMessage slowMessage = networkMessage.ReadMessage <SlowPowerupMessage> ();
		if (!BoundsChecker.checkValidBoundsTotal (slowMessage.x, bounds)) {
			Debug.Log ("Slow powerup in effect");
			activeBlockControl.setVelocity (new Vector3 (0, 0.5f, 0));
		}
	}

	// Update is called once per frame
	void Update () {

		//Only transform the local objects, othewise ignore
		if (isLocalPlayer) {

			//The following is related to the base elevate, let's see if this can be further optimised powerup
			if (activePowerUpCount > 0 && activePowerUpCount < 30) {
				
				activePowerUpCount += 1;
				gameObject.transform.Find ("base").gameObject.transform.position += new Vector3 (0, 1 * Time.deltaTime, 0);
				gameObject.transform.Find ("Base").gameObject.transform.position += new Vector3 (0, 1 * Time.deltaTime, 0);

			} else {
				activePowerUpCount = 0;
			}

			if (powerUpControl != null) {
				if (powerUpControl.getCollected()){
					if (powerUp.CompareTag ("power1")) {
						// do amazing powerup stuff here //
						activePowerUpCount += 1;
						
					} else if (powerUp.CompareTag("PowerUpSlow")) {
						//do powerup for slow
					}
					powerUpPresent = false;
					powerUpControl.DestoryPowerUp();
				}
			}

			if (activeBlockControl != null) {
				activeBlockControl.setVelocity (new Vector3 (0, -1, 0));

				if (BoundsChecker.checkValidBoundsLeft(activeBlock.transform,bounds[0])) {
					if (Input.GetKeyDown(KeyCode.LeftArrow)) {
						activeBlock.transform.position += new Vector3((float)-0.5, 0, 0);
					}	
				}

				if (BoundsChecker.checkValidBoundsRight(activeBlock.transform,bounds[1])) {
					if (Input.GetKeyDown(KeyCode.RightArrow)) {
						activeBlock.transform.position += new Vector3((float)0.5, 0, 0);
					}
				}

				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					activeBlock.transform.Rotate(0,0,-90);
					if (!(BoundsChecker.checkValidBoundsLeft(activeBlock.transform,bounds[0]) && BoundsChecker.checkValidBoundsRight(activeBlock.transform,bounds[1]))){
						activeBlock.transform.Rotate(0,0,90);
					}
				}

				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					activeBlockControl.applyDownwardForce ();
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

							if (BoundsChecker.checkValidBoundsLeft(activeBlock.transform,bounds[0]) && BoundsChecker.checkValidBoundsRight(activeBlock.transform,bounds[1])) {
								activeBlock.transform.Rotate (0, 0, -90);
							}

						// handle lateral movement
						} else if (t.phase == TouchPhase.Moved) {
							// user swiped left
							if (t.position.x - initialTouch.position.x < 0) {
								if (BoundsChecker.checkValidBoundsLeft(activeBlock.transform,bounds[0])) {
									activeBlock.transform.position += new Vector3 ((float)-0.5, 0, 0);
								}

							// user swiped right
							} else if (t.position.x - initialTouch.position.x > 0) {
								if (BoundsChecker.checkValidBoundsLeft(activeBlock.transform,bounds[0])) {
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
