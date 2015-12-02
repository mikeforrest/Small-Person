/*
 * Attached to Main Camera in each scene
 * 
 * Recenters Oculus Rift with Main Camera
 */

using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class RecenterOculus : MonoBehaviour {

	void Start () {
        InputTracking.Recenter();
	}
}
