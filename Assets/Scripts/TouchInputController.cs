using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputController : MonoBehaviour {

	public void pressLeftButton(){
		PlayerController.moveLeft = true;
	}

	public void pressRightButton(){
		PlayerController.moveRight = true;
	}

	public void pressRotateButton(){
		PlayerController.rotate = true;
	}
}
