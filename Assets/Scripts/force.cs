using UnityEngine;
using System.Collections;

public class force : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("force");
		gameObject.GetComponent<Rigidbody> ().AddForce (transform.forward * 1000);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
