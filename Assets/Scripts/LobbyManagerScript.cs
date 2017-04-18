using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManagerScript : NetworkLobbyManager {

	static public LobbyManagerScript s_Singleton;

	// Use this for initialization
	void Start () {
		s_Singleton = this;
	}
}
