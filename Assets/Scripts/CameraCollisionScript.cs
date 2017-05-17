using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionScript : MonoBehaviour {

	public GameObject maincharacter;
	public GameObject levelLimits;
	public Canvas UICanvas;
	public Camera mainCamera;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//Get bouds to check collisions 
		Bounds thisBounds = this.GetComponent<Collider2D> ().bounds;
		Bounds mainCharBounds = maincharacter.GetComponent<Collider2D> ().bounds;
		Bounds levelLimitsBounds = levelLimits.GetComponent<BoxCollider2D>().bounds;

		//Camera bounds is something extra :)
		//mainCamera.transform.position
		//mainCamera.orthographicSize
		Vector3 cameraCenter = mainCamera.transform.position;
		Vector3 cameraSize = new Vector3(Screen.width, Screen.height); //whhaaaat? 
		Bounds cameraBounds = new Bounds(cameraCenter,cameraSize);


		//Calculate camera view collider

		//New position for the camera trigger box (smal box around mainchar)
		float newX = this.transform.position.x; 
		float newY = this.transform.position.y; 

		//canvas.transform.position
		//canvas.pixelRect.width/2

		if (LeftBoundsCollision (mainCharBounds, thisBounds)) {
			newX = maincharacter.transform.position.x + (thisBounds.size.x/2 - mainCharBounds.size.x/2);
		}
		else if (RightBoundsCollision (mainCharBounds, thisBounds)) {
			newX = maincharacter.transform.position.x - (thisBounds.size.x/2 - mainCharBounds.size.x/2)+0.1f;
		}

		if (UpBoundsCollision (mainCharBounds, thisBounds)) {
			newY = mainCharBounds.center.y - (thisBounds.size.y/2 - mainCharBounds.size.y/2)+0.1f; //WHYYYYYYYY!!?!?!
		}
		else if (DownBoundsCollision (mainCharBounds, thisBounds)) {
			newY = mainCharBounds.center.y + (thisBounds.size.y/2 - mainCharBounds.size.y/2);
		}
			
		//Move the collision box to folow the mainchar
		this.transform.position = new Vector2 (newX, newY);

		//Move the camera to follow the collision box
		mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

		/*if (LeftBoundsCollision (cameraBounds, levelLimitsBounds)) {
			print ("LEFT");
		}*/
	}

	Bounds setCameraBounds()
	{
		Vector3 cameraCenter = mainCamera.transform.position;
		Vector3 cameraSize = new Vector3(Screen.width, Screen.height); //whhaaaat?  

		return new Bounds(cameraCenter,cameraSize);
	}

	bool LeftBoundsCollision(Bounds first, Bounds second)
	{
		//Returnes true if first bounds is enetering left wall of second bounds

		float firstLeftPositionX = first.center.x - (first.size.x / 2);
		float secondLeftPositionX = second.center.x - (second.size.x / 2);

		if (firstLeftPositionX <= secondLeftPositionX) {
			return true;
		}

		return false;
	}

	bool RightBoundsCollision(Bounds first, Bounds second)
	{
		//Returnes true if first bounds is enetering right wall of second bounds

		float firstRightPositionX = first.center.x + (first.size.x / 2);
		float secondRightPositionX = second.center.x + (second.size.x / 2);

		if (firstRightPositionX >= secondRightPositionX) {
			return true;
		}

		return false;
	}

	bool UpBoundsCollision(Bounds first, Bounds second)
	{
		//Returnes true if first bounds is enetering upper roof of second bounds

		float firstUpPositionY = first.center.y + (first.size.y / 2);
		float secondUpPositionY = second.center.y + (second.size.y / 2);

		if (firstUpPositionY >= secondUpPositionY) {
			return true;
		}

		return false;
	}

	bool DownBoundsCollision(Bounds first, Bounds second)
	{
		//Returnes true if first bounds is enetering lower floor of second bounds

		float firstDownPositionY = first.center.y - (first.size.y / 2);
		float secondDownPositionY = second.center.y - (second.size.y / 2);

		if (firstDownPositionY <= secondDownPositionY) {
			return true;
		}

		return false;
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject == maincharacter) {
			//The mainchar is moving out of the trigger box



		}
	}

}
