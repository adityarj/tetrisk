using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LobbyManagerUI : MonoBehaviour {

	public LobbyManagerScript lobbyManager;

	// Use this for initialization
	void Start () {
	}

	public void SetPort() {
		lobbyManager.networkPort = 7777;
	}

	public void SetIPAddress() {
		string IPAddress = GameObject.Find ("HostName").transform.FindChild ("Text").GetComponent<Text> ().text;

		if (IPAddress.Length == 0) {
			lobbyManager.networkAddress = "localhost";
		} else {
			lobbyManager.networkAddress = IPAddress;
		}
	}

	public void OnClickServer(){
		SetPort ();
		SetIPAddress ();
		lobbyManager.StartServer();
	}

	public void OnClickJoin(){
		SetPort ();
		SetIPAddress ();
		lobbyManager.StartClient ();
	}

	public void OnClickExit(){
		lobbyManager.StopHost ();
	}


}