using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour {

    public float timeBeforePossibleKeyInput = 3;

    private float timer;

	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {

        //update timer
        timer += Time.deltaTime;

        //Check timer
        if (timer >= timeBeforePossibleKeyInput)
        {
            //It's ok to press any keys now

            if (Input.anyKeyDown)
            {
                print("Back to main menu!");
                SceneManager.LoadScene("MainMenu");
            }

        }
	}
}
