using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		/*if (Input.anyKeyDown) { 
			SceneManager.LoadScene("Instructions");
		}*/
		
	}

	public void GoToPlayGame()
	{
		SceneManager.LoadScene("Level1");
	}

	public void GoToExitGame()
	{
		Application.Quit();
	}

	public void GoToInstructions()
	{
		SceneManager.LoadScene("Instructions");
	}

	public void GoToStory()
	{
		SceneManager.LoadScene("Story");
	}

	public void GoToCredits()
	{
		SceneManager.LoadScene("Credits");
	}
}
