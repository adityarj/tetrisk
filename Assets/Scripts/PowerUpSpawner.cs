using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject[] powerUpList;
	private int rand;
	public float[] powerUpProbs;

	private float maxHeight = 8f; // can go up to 12
	private float minHeight = 5f;

	private float minWidth = 1f;
	private float maxWidth = 3f;

	public GameObject getPowerUp() {
		rand = weightedProbability(powerUpProbs);
		return powerUpList[rand];
	}

	public Vector3 getLocation() {
		float x = Random.Range(minWidth, maxWidth);
		bool minus = (Random.Range(0,2) == 1);
		if (minus) {
			x *= -1;
		}

		float y = Random.Range(minHeight, maxHeight);
		Debug.Log(x);
		Debug.Log(y);

		return new Vector3 (x, y, 0);
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
