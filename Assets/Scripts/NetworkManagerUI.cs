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
}
