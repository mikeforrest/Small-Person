/*
 * Attached to WSC Canvas in Start Screen
 * Effect is consistent throughout the game
 * 
 * Locks and Hides Default Cursor
 * 
 */

using UnityEngine;
using System.Collections;

public class LockAndHideCursor : MonoBehaviour {
	
	void Start () {
		// Lock cursor
		//Cursor.lockState = CursorLockMode.Locked;

		// Hide cursor
		Cursor.visible = false;
	}
}
