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
	private float activeVel = -1;
	private bool spawnIsDisabled = false;
	private int iterVar = 0;

	//Related to winbar
	[SerializeField]
	private GameObject winBar;
	private GameObject localWinBar;
	[SerializeField]
	private GameObject gameOverUI;
	private GameObject gameOverUIInstance;

	//Related to PowerUps
	private GameObject powerUp;
	private PowerUpController powerUpControl;
	private PowerUpSpawner powerUpSpawner;
	private bool powerUpPresent;
	[SerializeField]
	private GameObject FortuneWheel;
	private FortuneWheelController fortuneWheelController;

	//Related to UI Buttons
	public static bool moveLeft;
	public static bool moveRight;
	public static bool rotate;

	// Use this for initialization
	void Start () {
		
		//Start spawner
		spawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z) + new Vector3 (0, 18, 0);
		bounds [1] = spawnPosition.x + 3.5;
		bounds [0] = spawnPosition.x - 3.5;

		blockSpawner = FindObjectOfType<BlockSpawner> ();
		powerUpSpawner = FindObjectOfType<PowerUpSpawner> ();

		//Instantiate all variables to false.
		moveLeft = false;
		moveRight = false;
		rotate = false;

		//If the script runs on a client, spawn for that client
		if (isLocalPlayer) {
			FortuneWheel = Instantiate (FortuneWheel);
			FortuneWheel.transform.position = new Vector3 (spawnPosition.x + 3, spawnPosition.y - 2, 0);
			fortuneWheelController = FortuneWheel.GetComponentInChildren<FortuneWheelController> ();
			SpawnBlock ();
			InvokeRepeating("SpawnPowerUp", 5f, 10f);
		}

	}

	private void SpawnPowerUp() {
		Debug.Log("present: " + powerUpPresent);
		if (!powerUpPresent){
			powerUpPresent = true;
			CmdSpawnPowerUp();
		}
	}

	//Command to the server to spawn a power-up over the network
	[Command]
	public void CmdSpawnPowerUp() {
		Debug.Log("Spawn powerup");

		Vector3 powerUpPos;
		do {
			powerUpPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z) + powerUpSpawner.getLocation();
		} while (Physics.CheckSphere(powerUpPos, 1f));

		powerUp = Instantiate(powerUpSpawner.getPowerUp());
		powerUp.transform.position = powerUpPos;

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

		if (isServer) {
			if (!WinBarSingleton.peekInstance ()) {
				this.CmdSpawnWinBar ();
			}
			NetworkServer.RegisterHandler (7999, OnReceiveEndGameMessage);

			NetworkServer.RegisterHandler (7998, OnReceiveSlowMessage);
			NetworkServer.RegisterHandler (7997, OnReceiveSpamMessage);
		}
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		if (isClient) {
			NetworkManager.singleton.client.RegisterHandler (7999, OnReceiveEndGameMessage);

			NetworkManager.singleton.client.RegisterHandler (7998, OnReceiveSlowMessage);
			NetworkManager.singleton.client.RegisterHandler (7997, OnReceiveSpamMessage);
		}

	}
	//This function exists in case more code needs to be put in SpawnBlock()
	public void SpawnBlock() {
		this.CmdSpawnBlock (this.spawnPosition);
	}

	public void SpawnSameBlock() {
		this.CmdSpawnBlock(this.spawnPosition);
	}

	//Function designed to wait and spawn blocks at regular intervals
	private void waitForTime() {
		if (this.iterVar >= 5) {
			this.iterVar = 0;
			this.spawnIsDisabled = false;
			CancelInvoke ();
			return;
		} else {
			Debug.Log ("Block is spawned");
			this.spawnIsDisabled = true;
			this.activeBlockControl = null;
			this.activeBlock.tag = "DeadBlock";
			this.SpawnBlock ();
			this.iterVar += 1;
		}
	}

	//Command to the server to spawn a block over the network
	[Command]
	public void CmdSpawnBlock(Vector3 position) {
		activeBlock = Instantiate(blockSpawner.getBlock());
		activeBlock.transform.position = position;
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

		Debug.Log (this.gameOverUI);

		if (this.gameOverUIInstance == null) {
			this.gameOverUIInstance = Instantiate (this.gameOverUI);
		}
	}

	//When a message is received to apply the slow powerup
	public void OnReceiveSlowMessage(NetworkMessage networkMessage) {
		Debug.Log ("Slow powerup");
		PowerupMessage slowMessage = networkMessage.ReadMessage <PowerupMessage> ();

		Debug.Log (slowMessage.x + " " + bounds[0]+"  "+bounds[1]);
		if (!BoundsChecker.checkValidBoundsTotal (slowMessage.x, bounds)) {
			Debug.Log ("Slow powerup in effect");
			activeVel = -0.2f;
			activeBlockControl.setVelocity (new Vector3 (0, -0.2f, 0));
		}
	}

	//When a message is received to apply the spam blocks powerup
	public void OnReceiveSpamMessage(NetworkMessage networkMessage) {
		Debug.Log ("Spam blocks message");
		PowerupMessage spamMessage = networkMessage.ReadMessage<PowerupMessage> ();

		Debug.Log (spamMessage.x + " " + + bounds[0]+"  "+bounds[1]);

		if (!BoundsChecker.checkValidBoundsTotal (spamMessage.x, bounds)) {
			Debug.Log ("Spam powerup in effect");
			this.spawnIsDisabled = true;
			InvokeRepeating ("waitForTime", 1.5f, 1.5f);

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
				//Debug.Log("collected: " + powerUpControl.getCollected());
				if (powerUpControl.getCollected()){
					//Debug.Log ("powerup: " + powerUp + " fortuneWheelState: " + fortuneWheelController.getState ());
					//Rotate according to powerup needs
					powerUpControl.setClient(NetworkManager.singleton.client);
					fortuneWheelController.HandlePowerup(powerUpControl);

					if (powerUp.CompareTag ("power1") && (fortuneWheelController.getState().Equals(PowerupState.Waiting))) {
						// do amazing powerup stuff here //
						activePowerUpCount += 1;
						
					} 
					Debug.Log("hi");
					powerUpPresent = false;
				}
			}

			if (activeBlockControl != null) {
				activeBlockControl.setVelocity (new Vector3 (0, activeVel, 0));

				if (BoundsChecker.checkValidBoundsLeft(activeBlock.transform,bounds[0])) {
					if (moveLeft) {
						activeBlock.transform.position += new Vector3((float)-0.5, 0, 0);
						moveLeft = false;
					}
				}

				if (BoundsChecker.checkValidBoundsRight(activeBlock.transform,bounds[1])) {
					if (moveRight) {
						activeBlock.transform.position += new Vector3((float)0.5, 0, 0);
						moveRight = false;
					}
				}

				if (rotate) {
					if (BoundsChecker.checkValidBoundsLeft(activeBlock.transform,bounds[0]) && BoundsChecker.checkValidBoundsRight(activeBlock.transform,bounds[1])) {
						activeBlock.transform.Rotate (0, 0, -90);
					}
					rotate = false;
				}

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

				if (Input.GetKeyDown (KeyCode.DownArrow) && this.activeVel!= 0.2f) {
					activeBlockControl.applyDownwardForce ();
				}

				//Debug.Log (this.spawnIsDisabled);
				//Spawn block if necessary based on the status player
				if (activeBlockControl.GetSpawnNext()) {
					this.activeVel = -1f;
					this.activeBlockControl = null;
					this.SpawnBlock ();
				}
			}
		}
	}
}
