/*
 * Attached to each selectable object
 * Uses WorldCrosshair cursor
 *
 * Features:
 * Changes object color on mouse over
 * Moves object with cursor while left mouse button is pressed
 * Rotates the object when right mouse button is pressed
 * Pushes/pulls the object with mouse wheel
 * Drops the object on mouse release
 *
 * Code written with Richard Barrett-Snyder
 *
 * How to check if object exists at a certain position (using Physics.Raycast):
 * http://forum.unity3d.com/threads/check-if-object-exists-at-a-certain-position.161462/
 */


using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

    public GameObject cursorObject; // Reference to Cursor GameObject in hierarchy
    WorldCrosshair cursor; // Reference to cursorObject's WorldCrosshair script

    Camera mainCam; // Reference to Main Camera
	public GameObject camObject;
	public GameObject empty;

    Vector3 offset; // Reference to difference between Cursor and Main Camera in 3D space
    float offsetMagnitude; // Reference to length of offset

    bool selected; // Whether this object is selected by the cursor
    static GameObject selectedObject = null; // Reference to the current selected object (can only be one since it's static)

    bool hittingBoundary; // Whether the object is touching an object in front of it (ex: wall)

    Rigidbody rb; // Reference to this object's rigidbody
    Material myMaterial; // Reference to my material
    Color myColor; // Reference to this object's color	

	bool collision = false;
	bool inPlane = false;
	Vector3 inPlaneCamPos;

	private Vector3 pOffset;

	void OnCollisionEnter (Collision target) {
		if( target.gameObject.tag.Equals("lid") == true ){
			collision = true;
		}
	}

	void OnCollisionExit(Collision target){
		if( target.gameObject.tag.Equals("lid") == true ){
			collision = false;
		}

	}

    void Awake()
    {
        mainCam = Camera.main;
        cursor = cursorObject.GetComponent<WorldCrosshair>();
    }

	void Start () {
        // The following initializations are in Start in case you wanted to Instantiate prefabs in an Awake function    
        rb = GetComponent<Rigidbody>();
		myMaterial = GetComponent<Renderer>().material;
		myColor = myMaterial.color;    
    }

	void Update () {

		//pOffset = mainCam.transform.position - gameObject.transform.position;
		if (inPlane == true) {
			empty.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);
		}

		bool hover;

		// Hover is true when the world cursor is on this object
		hover = (cursor.selectedObject == gameObject);

		// If cursor is on this object, highlight green
		myMaterial.color = hover ? Color.green : myColor;

		if (hover && Input.GetMouseButton (1) && (collision == false)) {
			ApplyForce();
		}

		if (hover && Input.GetKeyDown ("space")) {
			if (inPlane == false){ 

				//inPlaneCamPos = gameObject.transform.position + new Vector3(0, 1f, 0);
				//Debug.Log(inPlaneCamPos);
				//mainCam.transform.position = inPlaneCamPos;
				//mainCam.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);
				//mainCam.transform.SetParent(gameObject.transform,false);
				//mainCam.transform.localPosition = new Vector3(0,0,0);
				//mainCam.transform.localEulerAngles = new Vector3(0,0,0);

				inPlane = true;
			}
			
			else {
				Debug.Log("plane");
				//mainCam.transform.position = new Vector3(84.9f, 19.16f, -4.47f);
				//mainCam.transform.SetParent(null);
				inPlane = false;
					
			}
		}

		// If mouse is pressed when the cursor is on this object, and there is nothing else selected, it is now selected
		// Or, if this object was selected previously, it is still selected
		if (!selected && hover && Input.GetMouseButton(0) && selectedObject == null) {  // Only want to fire this once
			selected = true;
            selectedObject = gameObject;

			offset = cursor.transform.position - mainCam.transform.position;
			offsetMagnitude = offset.magnitude;

           rb.isKinematic = true; // Object won't move on its own
        }

		if (selected) {
            // If this object is selected, color it red
			myMaterial.color = Color.red;


			if (inPlane == false) {
            // If left mouse button clicked or pressed
            if (Input.GetMouseButton(0))
            {
                Vector3 t = cursor.transform.position - mainCam.transform.position;
                t = t.normalized * offsetMagnitude;

                Vector3 newPosition = mainCam.transform.position + t;

                // If there isn't another object at this position
                if (Physics.Raycast(rb.position, newPosition, 2) != true)
                {
                    // Move this object to the new position
                    rb.position = newPosition;
                    hittingBoundary = false;
                }
                else // If there is an object at this position
                {
                    hittingBoundary = true;
                }
            }
            else
            {
                // Deselect
                selected = false;
                selectedObject = null;

                rb.isKinematic = false; // Object will drop normally
            }

			// If the scroll wheel is used
			if (Input.GetAxis("Mouse ScrollWheel") != 0)
			{
                // Push object away
                // Assumes the user will only encounter a boundary moving the object forward
				if (Input.GetAxis("Mouse ScrollWheel") > 0 && ! hittingBoundary)
				{
					offsetMagnitude *= 1.05f;
				}
				
                // Pull object closer
				else if (Input.GetAxis("Mouse ScrollWheel") < 0)
				{
					offsetMagnitude /= 1.05f;
				}
			}

			// If right mouse button clicked or pressed
			if (Input.GetMouseButton(1))
			{
				// Rotate the object on y-axis
                // 2 can be replaced with any number
                transform.Rotate(0, rb.rotation.y + 2, 0);


            }
			}
		

		}
    }
	public void ApplyForce(){
		gameObject.GetComponent<Rigidbody> ().AddForce (transform.right * -220);
		Debug.Log ("force");
	}
}
