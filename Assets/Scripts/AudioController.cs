using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public static bool audioOn = true;

	void Start(){
		//initialized as on
		if (audioOn) {
			AudioListener.pause = false;
		} else {
			AudioListener.pause = true;
		}
	}
		
	public void ToggleAudio(){
		if (audioOn) {
			AudioListener.pause = true;
			audioOn = false;
		} else {
			AudioListener.pause = false;
			audioOn = true;
		}

	}
}
