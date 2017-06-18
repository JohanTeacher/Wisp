using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayOut : MonoBehaviour {

	public int HP; //How many petles does it take to take down barrier

	public bool isOpen; //If the way out is open. You may pass through it.


	WayOut(){

		HP = 3;
		isOpen = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenUp()
    {
        //opens upp the way out
        isOpen = true;

        //disable collider
        //transform.GetChild(0).GetComponent<Collider2D>().enabled = false;

        //start animation
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Open");
    }


}
