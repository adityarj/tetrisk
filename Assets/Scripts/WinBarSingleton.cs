using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WinBarSingleton {

	private static GameObject winBar;
	private static GameObject localWinBar;

	public static bool peekInstance() {
		return localWinBar != null;
	}

	public static GameObject getInstance(GameObject winBar) {
		if (localWinBar == null) {
			localWinBar = winBar;
			return localWinBar;
		} else {
			return localWinBar;
		}
	}

	public static void resetInstance() {
		localWinBar = null;
	}
}
