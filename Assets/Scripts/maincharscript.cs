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
	private GameObject isHangingFromHandle; //Pointer to handle from witch mainchar is hanging, NULL value means NOT hanging
	private bool onUnstableSurface; //If standing on an unstable surface (like a ball). Should enable jumping.
	private Vector2 startPosition; //Where does the mainchar start. Is set on start();

    private int petalsCollected; //How many petals that has been collected in total. More petals = more power.
	private GameObject pickUpObject; //Object that mainchar has picked up

    private GameObject glowy; //The object that holds the glow effect sprite


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
		isHangingFromHandle = null;
		pickUpObject = null;
	}

	// Use this for initialization
	void Start () {

		//Set start position
		startPosition = this.transform.position;
        glowy = transform.FindChild("glowthing").gameObject;

		//Set glowy start values
        glowy.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        Color tempColor = glowy.GetComponent<SpriteRenderer>().color;
        glowy.GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.4f);
	}
	
	// Update is called once per frame
	void Update () 
	{

		//Actions
		if (Input.GetKey (KeyCode.E) || Input.GetKey (KeyCode.Return)) {

			//Drop any potential items beeing picked up .. and..
			//Use your Flower Powers
			if (pickUpObject) {

				//Carrying around an object

				//Drop it
				pickUpObject.transform.localPosition = new Vector2(0,8);
				this.transform.GetChild(1).SetParent(null);
				if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
					pickUpObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(runSpeed*75, 200));
				else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
					pickUpObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(-(runSpeed*75), 200));
				else
					pickUpObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0, 200));
				pickUpObject.GetComponent<Rigidbody2D> ().AddTorque (50.0f);
				pickUpObject.GetComponent<PickUp> ().StateDropedDown ();
				GetComponent<CapsuleCollider2D> ().enabled = true;
				pickUpObject = null;

			}
		}


		//Jumping
		if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift) || Input.GetKeyDown (KeyCode.Space)) 
		{
			//Player wants to JUMP

			//Free mainchar from potential attachment to handles
			if(isHangingFromHandle)
			{
				//Get veclocity from the handle's velocity
				GetComponent<Rigidbody2D> ().velocity = isHangingFromHandle.GetComponent<Rigidbody2D> ().velocity*100;

				//Release
				isHangingFromHandle = null;
			}


			//Are we firmly on ground?
			if (jumpsMade < multiJumpNumber) {

				//Start jump sequence
				isJumping = true;
				jumpStartTime = Time.time;
				jumpsMade++;
				//GetComponent<Rigidbody2D> ().velocity = new Vector2(GetComponent<Rigidbody2D> ().velocity.x,0);
				GetComponent<Rigidbody2D> ().AddForce (new Vector2(0,jumpForce*100));
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

		//Check if hanging from handle
		if(isHangingFromHandle != null)
		{
			//Is hangging from a handle

			this.transform.position = isHangingFromHandle.transform.position;
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
			//this.transform.Translate (new Vector2 (0, (jumpForce * (jumpsMade+1)) * Time.deltaTime));
		
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

		//Reset glowy
		glowy.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
		Color tempColor = glowy.GetComponent<SpriteRenderer>().color;
		glowy.GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.4f);
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
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);

			print ("CLIMBING!!");
		} else if (other.gameObject.tag == "Petal") {
			// YAY! You just picked up a petal!

			//Destroy the petal graphics
			Destroy (other.gameObject);

			//Increase counter
			petalsCollected++;

			//Increase size of the glowiness. The Power of you
			glowy.transform.localScale += new Vector3 (0.15f, 0.15f, 0.15f);

			//Increase opasity of glowiness.
			Color tempColor = glowy.GetComponent<SpriteRenderer> ().color;
			glowy.GetComponent<SpriteRenderer> ().color = new Color (tempColor.r, tempColor.g, tempColor.b, tempColor.a + 0.2f); //by 0.2

			print ("ONE MORE PETAL!! Now you have " + petalsCollected + " flower petals.");
		} else if (other.gameObject.tag == "PendulumHandle") {
			//Hanging from a pendulum

			//set reference
			isHangingFromHandle = other.gameObject;

			print ("Hanging");
		} else if (other.gameObject.tag == "UpwardsInforcer") {
			//On a bouncer (like a trampoline) or a wind draft that will cast mainchar upwards

			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 0.0f);
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 4500));
			print ("UP UP UP!!");
		} else if (other.gameObject.tag == "WayOut") {
			//Enetering the way out to finish the level

			if (other.GetComponent<WayOut> ().isOpen) {
				print ("YAAAAY!! YOU'RE OUT! Congratulations, you made it.");
			} else {
				print ("Nope. You cant exit. There's something blocking it.");
			}
		} else if (other.gameObject.name == "JackInTheBox") {
			//Enetering space to enter windup key

			if (pickUpObject) {
				//is carrying something

				if (pickUpObject.name == "WindUpKey") {
					//is carrying the windup key!!! YAYA!

					//Place key in the box
					this.transform.GetChild(1).SetParent(other.gameObject.transform);
					pickUpObject.GetComponent<PickUp> ().StatePlaced ();
					pickUpObject.transform.localPosition = other.GetComponent<JackInTheBox> ().KeyLocalPosition;
					pickUpObject.transform.Rotate(0,0, other.GetComponent<JackInTheBox> ().KeyLocalRotation);
					pickUpObject = null;

				}
			}
		}

    }

	void OnTriggerExit2D(Collider2D other)
	{
		//Check exiting trigger collisions

		if (other.gameObject.tag == "Climbable") {

			//Starting to climb.
	
			//Back to falling and jumping and what not
			isClimbing = false;
			GetComponent<Rigidbody2D> ().gravityScale = 1.0f;

			print ("not climbing");
		}
		else if (other.gameObject.tag == "PendulumHandle")
		{
			//Hanging from a pendulum

			//set reference
			isHangingFromHandle = null;

			print ("Droped handle.");
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
		else if (coll.gameObject.tag == "PickUp") {
			//Picking up the pick up object when moving into it

			pickUpObject = coll.gameObject;
			coll.transform.SetParent (transform);
			coll.gameObject.GetComponent<PickUp> ().StatePickedUp();

			print ("Picking up " + coll.gameObject.name);
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
