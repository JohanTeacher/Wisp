using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour {

	public float rotationSpeed; //Maximum speed of rotation

	private float rotationRealSpeed;  //Actual speed each frame
	public float rotationAx; 		 //Speed axeleration
	public float rotationMaxAngle;

	private float currentRotation;

	// Use this for initialization
	void Start () {
		
		currentRotation = transform.rotation.z;
		rotationRealSpeed = rotationSpeed;
	}

	// Update is called once per frame
	void Update () {

		currentRotation = transform.rotation.eulerAngles.z;


		//Push ();

		//Calculate	 the rotation to create a pendulum effect
		if (currentRotation >= 0 && currentRotation <= rotationMaxAngle) {

			rotationRealSpeed -= rotationAx;
		
		} else if (currentRotation > (360-rotationMaxAngle) && currentRotation < 360) {

			rotationRealSpeed += rotationAx;
		
		}else {

			rotationRealSpeed *= -1;
		}
	

		//Do actual rotation of the frame
		transform.Rotate(new Vector3(0,0,rotationRealSpeed * Time.deltaTime));
	}
}
