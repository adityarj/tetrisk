using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject[] powerUpList;
	private int rand;
	public float[] powerUpProbs;

	public GameObject getPowerUp() {
		rand = weightedProbability(powerUpProbs);
		return powerUpList[rand];
	}

	public Vector3 getLocation() {
		return new Vector3 (2, 6, 0);
	}

	void Update () {
		Debug.Log(powerUpList[rand]);
	}

	private int weightedProbability(float[] powerUpProbs) {
		float total = 0;
		foreach (float elem in powerUpProbs) {
			total += elem;
		}

		float randomNum = Random.value * total;

		for (int i= 0; i < powerUpProbs.Length; i++) {
			if (randomNum < powerUpProbs[i]) {
				return i;
			}
			else {
				randomNum -= powerUpProbs[i];
			}
		}
		return powerUpProbs.Length - 1;
	}
 }
