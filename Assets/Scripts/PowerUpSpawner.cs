using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject[] powerUpList;

	public GameObject getPowerUp() {
		return powerUpList[Random.Range(0,powerUpList.Length)];
	}
 }
