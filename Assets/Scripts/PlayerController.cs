﻿using System.Collections;
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
	private float activeVel = -3f;
	private bool spawnIsDisabled = false;
	private int iterVar = 0;

	//Related to winbar
	[SerializeField]
	private GameObject winBar;
	private GameObject localWinBar;
	[SerializeField]
	private GameObject gameOverUI;
	private GameObject gameOverUIInstance;
	[SerializeField]
	private GameObject gameWinUI;
	private GameObject gameWinUIInstance;
	private bool EndFlag = false;

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
			InvokeRepeating("SpawnPowerUp", 10f, 20f);
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
		}
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		if (isClient) {
			NetworkManager.singleton.client.RegisterHandler (7999, OnReceiveEndGameMessage);
			NetworkManager.singleton.client.RegisterHandler (7998, OnReceiveSlowMessage);
		}

	}
	//This function exists in case more code needs to be put in SpawnBlock()
	public void SpawnBlock() {
		this.CmdSpawnBlock (this.spawnPosition);
	}

	public void SpawnSameBlock() {
		this.CmdSpawnBlock(this.spawnPosition);
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
		this.localWinBar.transform.position = new Vector3 (0, 9, 0);
		NetworkServer.Spawn (this.localWinBar);
	}

	[Command]
	public void CmdReceiveEndGameMessage(bool endCond) {
		if (!this.EndFlag) {
			WinBarSingleton.resetInstance ();
			if (endCond) {
				Instantiate (this.gameWinUI);
			} else {
				Instantiate (this.gameOverUI);
			}
			this.EndFlag = true;
		}
	}
	//When a message is received to end the game
	public void OnReceiveEndGameMessage(NetworkMessage networkMessage) {
		
		EndGameMessage endgame = networkMessage.ReadMessage<EndGameMessage> ();
		if (isLocalPlayer) {
			Debug.Log ("IsLocalPlayer");
			if (BoundsChecker.checkValidBoundsTotal (endgame.x, bounds)) {
				Instantiate (this.gameWinUI);
				this.CmdReceiveEndGameMessage (false);
			} else {
				Instantiate (this.gameOverUI);
				this.CmdReceiveEndGameMessage (true);
			}
		}
	}

	//When a message is received to apply the slow powerup
	public void OnReceiveSlowMessage(NetworkMessage networkMessage) {
		PowerupMessage slowMessage = networkMessage.ReadMessage <PowerupMessage> ();

		Debug.Log (slowMessage.x + " " + bounds[0]+"  "+bounds[1]);
		if (!BoundsChecker.checkValidBoundsTotal (slowMessage.x, bounds)) {
			Debug.Log ("Slow powerup in effect");
			activeVel = -0.2f;
			activeBlockControl.setVelocity (new Vector3 (0, -0.2f, 0));
		}
	}

	// Update is called once per frame
	void Update () {

		//Only transform the local objects, othewise ignore
		if (isLocalPlayer) {

			if (fortuneWheelController.getState() == PowerupState.Refreshing) {
				powerUpPresent = false;
			}

			if (powerUpControl != null) {
				if (powerUpControl.getCollected ()) {
					//Rotate according to powerup needs
					powerUpControl.setClient (NetworkManager.singleton.client);
					fortuneWheelController.HandlePowerup (powerUpControl);				
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

				//Spawn block if necessary based on the status player
				if (activeBlockControl.GetSpawnNext()) {
					this.activeBlockControl = null;
					this.SpawnBlock ();
				}
			}
		}
	}
}
