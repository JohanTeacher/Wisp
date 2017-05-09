using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	public Vector2 PickedUpLocalPosition; //How is the object positioned towards mainchar when it has been picked up 
	public float PickedUpLocalRotation; //How is the object rotated when picked up 

	public Vector2 LyingAroundLocalPosition; //How is it positioned when lying around
	public float LyingAroundLocalRotation; //How is it rotated when lying around

	PickUp()
	{
		PickedUpLocalPosition.Set (0, 0); 
		PickedUpLocalRotation = 0;

		LyingAroundLocalPosition.Set (0, 0); 
		LyingAroundLocalRotation = 0;
	}

	// Use this for initialization
	void Start () {

		LyingAroundLocalPosition.Set (0, 0); 
		LyingAroundLocalRotation = transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StatePickedUp()
	{
		//Position and rotate to be picked up
		transform.localPosition = PickedUpLocalPosition;
		//transform.localRotation.ToAngleAxis(PickedUpLocalRotation,Vector3.forward);
		transform.rotation = new Quaternion(0,0,0,0);
		transform.Rotate (new Vector3 (0,0,PickedUpLocalRotation));
		//GetComponent<CapsuleCollider2D> ().enabled = false;
		GetComponent<CapsuleCollider2D>().isTrigger = true;
		//GetComponent<Rigidbody2D> ().gravityScale = 0;
		//GetComponent<Rigidbody2D> ().Sleep ();
		GetComponent<Rigidbody2D> ().simulated = false;
	}

	public void StateDropedDown()
	{
		//Position and rotate to lay around
		//transform.localPosition = LyingAroundLocalPosition;
		//transform.localRotation.ToAngleAxis(LyingAroundLocalRotation,Vector3.forward);
		//transform.rotation.Set(0,0,0,0);
		//transform.Rotate (new Vector3 (0,0,LyingAroundLocalRotation));
		//GetComponent<CapsuleCollider2D> ().enabled = true;
		GetComponent<CapsuleCollider2D>().isTrigger = false;
		GetComponent<Rigidbody2D> ().simulated = true;//gravityScale = 1;
	}

	public void StatePlaced ()
	{
		//Position and rotate as it should be when in the right place (key i the hole?)
		//Position and rotations should be set in the other object

		transform.rotation = new Quaternion(0,0,0,0);
		GetComponent<CapsuleCollider2D> ().enabled = false;
		//GetComponent<CapsuleCollider2D>().isTrigger = true;
		GetComponent<Rigidbody2D> ().gravityScale = 0;
		GetComponent<Rigidbody2D> ().Sleep ();
	}



}
