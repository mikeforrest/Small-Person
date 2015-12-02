using UnityEngine;
using System.Collections;

public class collision : MonoBehaviour {
	public GameObject partSys1;
	public GameObject partSys2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter() {

		gameObject.GetComponent<Rigidbody> ().useGravity = false;
		Destroy (gameObject.GetComponent<Interactable>());
		gameObject.transform.position = new Vector3 (4.51f, 3.13f, 5.83f);
		gameObject.GetComponent<Renderer> ().material.color = Color.white;
	}
}
