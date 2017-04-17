using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkManagerUI : NetworkManager {

	public void StartupHost() {
		SetPort ();
		SetIPAddress ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinClient() {
		SetPort ();
		SetIPAddress ();
		NetworkManager.singleton.StartClient ();
	}

	public void SetPort() {
		NetworkManager.singleton.networkPort = 7777;
	}

	public void SetIPAddress() {
		string IPAddress = GameObject.Find ("HostName").transform.FindChild ("Text").GetComponent<Text> ().text;
		NetworkManager.singleton.networkAddress = IPAddress;
	}

	void OnLevelWasLoaded(int level){
		if (level == 0) {
			SetUpMenuSceneButtons ();
		} else {
			SetUpGameSceneButtons ();
		}
	}

	void SetUpMenuSceneButtons(){
		GameObject.Find ("HostButton").GetComponent<Button> ().onClick.RemoveAllListeners();
		GameObject.Find ("HostButton").GetComponent<Button> ().onClick.AddListener(StartupHost);

		GameObject.Find ("JoinButton").GetComponent<Button> ().onClick.RemoveAllListeners();
		GameObject.Find ("JoinButton").GetComponent<Button> ().onClick.AddListener(JoinClient);
	}

	void SetUpGameSceneButtons(){
		GameObject.Find ("DisconnectButton").GetComponent<Button> ().onClick.RemoveAllListeners();
		GameObject.Find ("DisconnectButton").GetComponent<Button> ().onClick.AddListener(NetworkManager.singleton.StopHost);
	}

	public void Disconnect() {
		NetworkManager.singleton.StopHost ();
	}

	public void Exit() {
		Application.Quit ();
	}
}
