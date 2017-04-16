using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	//initialized as on
	bool audioOn = true;

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
