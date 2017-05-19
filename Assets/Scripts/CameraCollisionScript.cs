using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionScript : MonoBehaviour {

	public GameObject maincharacter;
	public GameObject levelLimits;
	public Camera mainCamera;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


		//Get bouds to check collisions 
		Bounds thisBounds = this.GetComponent<Collider2D> ().bounds;
		Bounds mainCharBounds = maincharacter.GetComponent<Collider2D> ().bounds;
		/*
		Bounds levelLimitsBounds = levelLimits.GetComponent<BoxCollider2D>().bounds;
		Bounds cameraBounds;

		//Camera bounds is a bit trickier to calculate
		//mainCamera.orthographicSize ?? Do I need to use that? Maybe not?
		Vector3 cameraCenter = mainCamera.ScreenToWorldPoint (new Vector3(Screen.width/2, Screen.height/2, 0));
		Vector3 topLeftOfScreenInWorld = mainCamera.ScreenToWorldPoint (new Vector3(0, 0, 0));
		Vector3 rightOfScreenInWorld = mainCamera.ScreenToWorldPoint (new Vector3(Screen.width, 0, 0));
		Vector3 bottomOfScreenInWorld = mainCamera.ScreenToWorldPoint (new Vector3(0, Screen.height, 0));
		float screenWidthInWorld = Vector3.Distance (topLeftOfScreenInWorld, rightOfScreenInWorld);
		float screenHeightInWorld = Vector3.Distance (topLeftOfScreenInWorld, bottomOfScreenInWorld);
		Vector3 cameraSize = new Vector3 (screenWidthInWorld,screenHeightInWorld,0);
		cameraBounds = new Bounds(cameraCenter,cameraSize);
		*/


		//Calculate camera trigger box position
		//--------------------------------------

		//New position for the camera trigger box (smal box around mainchar)
		float newX = this.transform.position.x; 
		float newY = this.transform.position.y; 


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



		//Calculate camera position
		//-------------------------

		//Variables x, y, z for camera
		float cameraX, cameraY, cameraZ;

		//First set camera position at the trigger box
		cameraX = transform.position.x; 
		cameraY = transform.position.y; 
		cameraZ = mainCamera.transform.position.z;
		mainCamera.transform.position = new Vector3 (cameraX, cameraY, cameraZ);


		/*	
		//Then, if the camera bounds hits the walls of the level bounds...
		//... Place the camera back on the border

		if (LeftBoundsCollision (cameraBounds, levelLimitsBounds)) {
			cameraX = topLeftOfScreenInWorld.x + (screenWidthInWorld / 2);
			print ("LEFT");
		} else if (RightBoundsCollision (cameraBounds, levelLimitsBounds)) {
			cameraX = rightOfScreenInWorld.x - (screenWidthInWorld / 2);
			print ("RIGHT");
		}

		if (DownBoundsCollision (cameraBounds, levelLimitsBounds)) {
			cameraY = topLeftOfScreenInWorld.y + (screenHeightInWorld / 2);
			print ("BOTTOM");
		} else if (UpBoundsCollision (cameraBounds, levelLimitsBounds)) {
			cameraY = bottomOfScreenInWorld.y - (screenHeightInWorld / 2);
			print ("TOP");
		}

		mainCamera.transform.position = new Vector3 (cameraX, cameraY, cameraZ);
		*/
			
	}

	Bounds setCameraBounds()
	{
		Vector3 cameraCenter = mainCamera.transform.position;
		Vector3 cameraSize = new Vector3(Screen.width, Screen.height); //whhaaaat?  

		return new Bounds(cameraCenter,cameraSize);
	}

	bool isInside(Bounds smaller, Bounds larger)
	{
		//Returns true when smaller Bounds is copletely inside the larger one

		if (LeftBoundsCollision (smaller, larger) || RightBoundsCollision (smaller, larger) ||
			UpBoundsCollision (smaller, larger) || DownBoundsCollision (smaller, larger)) {

			return false;
		}

		return true;
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
