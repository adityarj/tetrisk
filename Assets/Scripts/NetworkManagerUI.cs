using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkManagerUI : NetworkManager {

	public void StartupHost() {
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinClient() {
		SetPort ();
		IPAddress ();
		NetworkManager.singleton.StartClient ();
	}

	public void SetPort() {
		NetworkManager.singleton.networkPort = 7777;
	}

	public void IPAddress() {
		string IPAddress = "localhost";
		NetworkManager.singleton.networkAddress = IPAddress;
	}
}
