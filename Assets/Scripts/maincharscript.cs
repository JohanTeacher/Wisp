using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class maincharscript : MonoBehaviour {

	//Public variables
	public float runSpeed;	//How fast are character running
	public float jumpForce;	//How fast is it jumping
	public float jumpDurationTime;	//How many seconds is the force being aplied
	public float jumpCooldown; 	 //How long until next jump is possible
	public float deathPositionY; //At what hight position do you die from falling
	public int multiJumpNumber;  //How many jumps can be made sequence
	public float powerDurationTime; //How many seconds is the power going to be showed?

	public Canvas UICanvas; //Reference to the UI


	//Private variables
	private int jumpsMade;	//Itereator. How many jumps ha pre
	private float jumpStartTime; //When the jump was called
	private float powerStartTime; //When the power was called
	private bool isFalling;
	private bool isJumping;
	private bool isClimbing;
	private bool isUsingPower;
	private GameObject isCloseToWayOut; //true if close by a way out. Using powers may open the way.
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
		powerDurationTime = 1.0f;
		isFalling = true;
		isJumping = false;
		isClimbing = false;
		isUsingPower = false;
		isCloseToWayOut = null;
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
	
		//Hide petals in UI
		for (int i = 2; i < 5; i++) {
			UICanvas.transform.GetChild (i).GetComponent<CanvasRenderer> ().SetAlpha (0);
		}
		//Hide glowy in UI
		UICanvas.transform.GetChild (0).GetComponent<CanvasRenderer> ().SetAlpha (0.40f);
		UICanvas.transform.GetChild (0).transform.localScale = new Vector2(0.4f,0.4f);

		//Hide power animation
		this.transform.FindChild ("magic").GetComponent<SpriteRenderer> ().enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		//Actions
		if (Input.GetKey (KeyCode.E) || Input.GetKey (KeyCode.Return)) {

			//Drop any potential items beeing picked up .. and..
			//Use your Flower Powers

			//Catrying anything?
			if (pickUpObject) {

				//Carrying around an object


				if (pickUpObject.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), new Collider2D[3]) <= 1){

					//Drop it

					pickUpObject.transform.localPosition = new Vector2 (0, 8);
					this.transform.GetChild (1).SetParent (null);
					if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
						pickUpObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (runSpeed * 75, 200));
					else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
						pickUpObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-(runSpeed * 75), 200));
					else
						pickUpObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 200));
					pickUpObject.GetComponent<Rigidbody2D> ().AddTorque (50.0f);
					pickUpObject.GetComponent<PickUp> ().StateDropedDown ();
					GetComponent<CapsuleCollider2D> ().enabled = true;
					pickUpObject = null;
				
				} else {
					print (pickUpObject.name + " is touching something.");
				}

			}

			//Use your power
			StartPowerAnimation();


			//Is the power affecting any doorways?
			if (isCloseToWayOut) {
				//Standing by a way out when using the power

				//Do you have enough power?
				if(petalsCollected >= isCloseToWayOut.GetComponent<WayOut>().HP)
				{
					print ("YOU HAVE THE POWA!! THE WAY OPENS!");
					isCloseToWayOut.GetComponent<WayOut> ().isOpen = true;

                    //Try to walk through the WayOut
                    TryToWalkThroughWayOut(isCloseToWayOut);
				}
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

            //Change to running animation
            GetComponent<Animator>().SetBool("Walking", true);

        }
		else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) 
		{
			//Player wants to move LEFT
			this.transform.Translate (new Vector2 (-runSpeed * Time.deltaTime, 0));

			//Flip sprite
			this.GetComponent<SpriteRenderer>().flipX = true;

            //Change to running animation
            GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            //Reset Animation
            GetComponent<Animator>().SetBool("Walking", false);

			//Unflip
			this.GetComponent<SpriteRenderer>().flipX = false;
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

		//Update Power-animation
		PowerUpdate();

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

	void PowerUpdate()
	{
		//If powersequence is activated
		if(isUsingPower)
		{
			//if timer is < powertime
			if (Time.time - powerStartTime < powerDurationTime) {
				//Play animation
				this.transform.FindChild ("magic").GetComponent<SpriteRenderer> ().enabled = true;
			}
			else
			{
				//dectivate power
				isUsingPower = false;

				this.transform.FindChild ("magic").GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}

	void StartPowerAnimation()
	{
		//Reset timer
		powerStartTime = Time.time;

		//Set isUsingPower
		isUsingPower = true;
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

			//Change color of the glowthing
			glowy.GetComponent<SpriteRenderer>().color = other.transform.GetChild(0).GetComponent<SpriteRenderer>().color;

			//Increase opasity of glowiness.
			Color tempColor = glowy.GetComponent<SpriteRenderer> ().color;
			glowy.GetComponent<SpriteRenderer> ().color = new Color (tempColor.r, tempColor.g, tempColor.b, tempColor.a + 0.2f); //by 0.2

			//Reset jumping
			isFalling = false;
			jumpsMade = 0;

			//Add flower to the UI
			UICanvas.transform.FindChild (other.gameObject.name).GetComponent<CanvasRenderer> ().SetAlpha (1);

			//Increase glowy intensity and change color
			//float newGlowyAlpha = UICanvas.transform.GetChild (0).GetComponent<CanvasRenderer> ().GetAlpha () + 0.2f;
			//UICanvas.transform.GetChild (0).GetComponent<CanvasRenderer> ().SetAlpha (newGlowyAlpha);
			Vector2 glowyScale = UICanvas.transform.GetChild (0).transform.localScale;
			UICanvas.transform.GetChild (0).transform.localScale = new Vector2(glowyScale.x + 0.2f, glowyScale.y + 0.2f);
			UICanvas.transform.GetChild (0).GetComponent<CanvasRenderer>().SetColor(other.transform.GetChild(0).GetComponent<SpriteRenderer>().color);

			print ("ONE MORE PETAL!! Now you have " + petalsCollected + " flower petals.");
		} else if (other.gameObject.tag == "PendulumHandle") {
            //Hanging from a pendulum
            isFalling = false;
            isJumping = false;
            jumpsMade = 0;

            //set reference
            isHangingFromHandle = other.gameObject;

			GetComponent<Rigidbody2D> ().gravityScale = 0.0f;

			print ("Hanging");
		} else if (other.gameObject.tag == "UpwardsInforcer") {
			//On a bouncer (like a trampoline) or a wind draft that will cast mainchar upwards

			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 0.0f);
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 4500));
			print ("UP UP UP!!");
		} else if (other.gameObject.tag == "WayOut") {
			//Enetering the way out to finish the level

            //Mark what way out mainchar is close to
			isCloseToWayOut = other.gameObject;

            //Try to walk through
            TryToWalkThroughWayOut(isCloseToWayOut);
           

        } else if (other.gameObject.name == "JackInTheBox") {
			//Enetering space to enter windup key

			if (pickUpObject) {
				//is carrying something

				if (pickUpObject.name == "WindUpKey") {
					//is carrying the windup key!!! YAYA!

					//Place key in the box
					this.transform.FindChild("WindUpKey").SetParent(other.gameObject.transform);
					pickUpObject.GetComponent<PickUp> ().StatePlaced ();
					pickUpObject.transform.localPosition = other.GetComponent<JackInTheBox> ().KeyLocalPosition;
					pickUpObject.transform.Rotate(0,0, other.GetComponent<JackInTheBox> ().KeyLocalRotation);
					pickUpObject = null;

					//Set the timer
					other.GetComponent<JackInTheBox> ().Unlock();

				}
			}
		}

    }

    void TryToWalkThroughWayOut(GameObject other)
    { 
       //Try to walk through the WayOut
        if (other.GetComponent<WayOut>().isOpen)
        {
            print("YAAAAY!! YOU'RE OUT! Congratulations, you made it.");
            SceneManager.LoadScene("Level2");
        }
        else {
            print("Nope. You cant exit. There's something blocking it.");
        }
    }

	void OnTriggerExit2D(Collider2D other)
	{
		//Check exiting trigger collisions

		if (other.gameObject.tag == "Climbable") {

			//No longer to climbing.
	
			//Back to falling and jumping and what not
			isClimbing = false;
			GetComponent<Rigidbody2D> ().gravityScale = 1.0f;

			print ("not climbing");
		} else if (other.gameObject.tag == "PendulumHandle") {
			//No longer hanging from a pendulum

			//set reference
			isHangingFromHandle = null;

			GetComponent<Rigidbody2D> ().gravityScale = 1.0f;

			print ("Droped handle.");
		} else if (other.gameObject.tag == "WayOut") {
			//No longer close to the way out

			isCloseToWayOut = null;
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
