using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameOverUI : MonoBehaviour {

	public void Quit() {
		Debug.Log ("Exit");
	}

	public void PlayAgain() {
		Debug.Log ("Try again");
		NetworkManager.Shutdown ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
