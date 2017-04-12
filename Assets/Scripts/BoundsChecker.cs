using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsChecker {

	public static bool checkValidBoundsRight(Transform transform, double bound) {
		foreach (Transform child in transform) {
			if (child.position.x >= bound) {
				return false;
			}
		}
		return true;
	}

	public static bool checkValidBoundsLeft(Transform transform,double bound) {
		foreach (Transform child in transform) {
			if (child.position.x <= bound) {
				return false;
			}
		}
		return true;
	}
		
	public static bool checkValidBoundsTotal(double x, double[] bounds) {
		return (x <= bounds[1] && x>= bounds[0]);
	}
}
