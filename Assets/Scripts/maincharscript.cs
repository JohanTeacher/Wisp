using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maincharscript : MonoBehaviour {

	public float runSpeed;	//How fast are character running
	public float jumpForce;	//How fast is it jumping
	public float jumpDurationTime;	//How many seconds is the force being aplied
	public float jumpCooldown; 	 //How long until next jump is possible
	public float deathPositionY; //At what hight position do you die from falling
	public int multiJumpNumber;  //How many jumps can be made sequence
	private int jumpsMade;	//Itereator. How many jumps ha pre
	private float jumpStartTime; //When the jump was called
	private bool isFalling;
	private bool isJumping;
	private bool isClimbing;
	private bool onUnstableSurface; //If standing on an unstable surface (like a ball). Should enable jumping.
	private Vector2 startPosition; //Where does the mainchar start. Is set on start();

	maincharscript()
	{
		runSpeed = 0.0f;
		jumpForce = 0.0f;
		jumpDurationTime = 1.0f;
		jumpCooldown = 2.0f;
		jumpStartTime = 0.0f;
		deathPositionY = -1000.0f;
		jumpsMade = 0;
		multiJumpNumber = 2;
		isFalling = true;
		isJumping = false;
		isClimbing = false;
	}

	// Use this for initialization
	void Start () {

		//Set start position
		startPosition = this.transform.position;

	}
	
	// Update is called once per frame
	void Update () 
	{

		//Jumping
		if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift) || Input.GetKeyDown (KeyCode.Space)) 
		{
			//Player wants to JUMP

			//Are we firmly on ground?
			if (jumpsMade < multiJumpNumber) {

				//Start jump sequence
				isJumping = true;
				jumpStartTime = Time.time;
				jumpsMade++;
				print ("JUMP!");
			}
		}



		//Side movement
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) 
		{
			//Player wants to move RIGHT
			this.transform.Translate (new Vector2 (runSpeed * Time.deltaTime, 0));

			//Flip sprite
			this.GetComponent<SpriteRenderer>().flipX = false;

		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) 
		{
			//Player wants to move LEFT
			this.transform.Translate (new Vector2 (-runSpeed * Time.deltaTime, 0));

			//Flip sprite
			this.GetComponent<SpriteRenderer>().flipX = true;
		}

		//Up/down movement
		if (isClimbing) {
			//Can only be done if in a climbing state

			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
				//Player wants to move UP
				this.transform.Translate (new Vector2 (0, runSpeed * Time.deltaTime));

			}
			if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
				//Player wants to move Down
				this.transform.Translate (new Vector2 (0,-runSpeed * Time.deltaTime));

			}
		}

	}

	void FixedUpdate()
	{
		//Update jump
		JumpUpdate();

		//Check if we've fallen too far
		if (this.transform.position.y <= deathPositionY) {

			//Falling to death

			print("DIE!!!!");
			//Reset to startposition
			resetAll();
		}
	}

	void JumpUpdate()
	{
		//Update all necessary data to do the JUMP sequence

		//Update isFalling with current falling or not falling state
		if (GetComponent<Rigidbody2D> ().velocity.y == 0 || onUnstableSurface) {
			isFalling = false;
			jumpsMade = 0;
		} else {
			isFalling = true;
		}

		//Do the jump sequence
		if (isJumping) {

			//Falling and in a jump

			//Continue Jump movement
			this.transform.Translate (new Vector2 (0, (jumpForce * (jumpsMade+1)) * Time.deltaTime));
		
		} else if (!isFalling && isJumping) {

			//Has stoped falling (e.g. by hiting the ground)

			//Is no longer jumping
			isJumping = false;

			print ("NO JUMP!!");
		}

		if (isJumping && (Time.time - jumpStartTime) > jumpDurationTime) {

			//Jump timer ends. Stop aplying force upwards.
			isJumping = false;


		}


		// Falling but not jumping (e.g. falling off a cliff) : Do nothing about jump
		// Not Faling Not jumping (e.g. walking): Do nothing about jump 
	}


	void resetAll()
	{
		//Resets all data to original state

		//Reset falling and jumping flags
		isFalling = true;
		isJumping = false;

		//Reset position
		this.transform.position = startPosition;

		//Reset velocity
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Check tigger collisions

		if (other.gameObject.tag == "Climbable") {

			//Starting to climb.

			//Stop falling and jumping and what not
			isFalling = false;
			isJumping = false;
			isClimbing = true;
			GetComponent<Rigidbody2D> ().gravityScale = 0;
			GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);

			print ("CLIMBING!!");
		} 

	}

	void OnTriggerExit2D(Collider2D other)
	{
		//Check exiting trigger collisions

		if (other.gameObject.tag == "Climbable") {

			//Starting to climb.
	
			//Back to falling and jumping and what not
			isClimbing = false;
			GetComponent<Rigidbody2D> ().gravityScale = 0.5f;

			print ("not climbing");
		}

	}

	void OnCollisionEnter2D(Collision2D coll) {

		//Check colliding with some collidable objects

		if (coll.gameObject.tag == "Ball") {

			//Colliding with a ball

			//mark surface unstable so that jumping might work
			onUnstableSurface = true;

			print ("ON BALL!!");
		}
			

	}

	void OnCollisionExit2D(Collision2D coll) {

		//Check if no longer on the collidable

		if (coll.gameObject.tag == "Ball") {

			//No longer colliding with the ball
			onUnstableSurface = false;

			print ("Off ball.");
		}


	}
}
