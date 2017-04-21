using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionScript : MonoBehaviour {

	public GameObject maincharacter;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//Get bouds to check collision of camera view collider
		Bounds thisBounds = this.GetComponent<Collider2D> ().bounds;
		Bounds mainCharBounds = maincharacter.GetComponent<Collider2D> ().bounds;

		//New position for the camera
		float newX = this.transform.position.x; 
		float newY = maincharacter.transform.position.y;

		if(!thisBounds.Intersects(mainCharBounds)){
			//mainchar is outside of the collider box!!

			//On what side of the box?
			if (maincharacter.transform.position.x < this.transform.position.x - (this.GetComponent<Collider2D> ().bounds.size.x / 2)) {
				//mainchars' on the LEFT side of collider
				newX = maincharacter.transform.position.x + (this.GetComponent<Collider2D> ().bounds.size.x / 2) + (maincharacter.GetComponent<SpriteRenderer> ().bounds.size.x/2.2f);
			
			} 
			else if (maincharacter.transform.position.x > this.transform.position.x - (this.GetComponent<Collider2D> ().bounds.size.x / 2)) {
				//mainchar's on the RIGHT side of collider
				newX = maincharacter.transform.position.x - (this.GetComponent<Collider2D> ().bounds.size.x / 2) - (maincharacter.GetComponent<SpriteRenderer> ().bounds.size.x/1.7f);
			
			}
			/*if (maincharacter.transform.position.y > this.transform.position.y - (this.GetComponent<Collider2D> ().bounds.size.y / 2)) {
				//mainchar's OVER the collider
				//newY = maincharacter.transform.position.y - (this.GetComponent<Collider2D> ().bounds.size.y / 2) - (maincharacter.GetComponent<SpriteRenderer> ().bounds.size.y/2);
			
			} 
			else if (maincharacter.transform.position.y < this.transform.position.y - (this.GetComponent<Collider2D> ().bounds.size.y / 2)) {
				//mainchar is UNDER the collider
				//newY = maincharacter.transform.position.y + (this.GetComponent<Collider2D> ().bounds.size.y / 2) + (maincharacter.GetComponent<SpriteRenderer> ().bounds.size.y/2);
			
			}*/

		}

		//Move the camera to folow the mainchar
		this.transform.position = new Vector2 (newX, newY);

	}


	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject == maincharacter) {
			//The mainchar is moving out of the trigger box



		}
	}

}
