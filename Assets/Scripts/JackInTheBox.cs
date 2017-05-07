using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInTheBox : MonoBehaviour {

	public Vector2 KeyLocalPosition; //How is the key positioned placed
	public float KeyLocalRotation; //How is the key rotated when placed 
	public float countDownClock; //Clock counting down until the box will open and shoot you up
	public float timeToOpening; //How many seconds until box will open up
	public float catapultPower; //How much power to add when shooting up objects
	public bool locked; //timer will only tick if the box is unlocked

	//Overlaping objects ti boost
	ContactFilter2D filter;
	Collider2D[] overlapingColliders = new Collider2D[3];

	// Use this for initialization
	void Start () {

		countDownClock = 0.0f;
		locked = true;
	}
	
	// Update is called once per frame
	void Update () {

		//Update timer
		if (!locked)
			countDownClock += Time.deltaTime;

		//Check time
		if (countDownClock >= timeToOpening) {
			// It's time to open up the lid and shot up any objects ontop of it

			//Find items that recides ontop of the box
			int number = GetComponentInChildren<BoxCollider2D>().OverlapCollider(filter,overlapingColliders);

			print("Boost " + number + " Colliders precent:");

			//Loop through the collider list 
			//and shoot them up in the air
			for (int i = 0; i < number; i++) {
				overlapingColliders[i].attachedRigidbody.AddForce (new Vector2(0, catapultPower));
			}

			//Resetclock
			countDownClock = 0;
		}
	}

	public void Lock()
	{
		locked = true;
	}

	public void Unlock()
	{
		locked = false;
	}
}
