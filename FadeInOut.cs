using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

	public GUITexture fadeScreen;
	public bool isEnd;
	private float time;
	
	void Start () {
		time = 1f;
		//make sure the black screen covers the screen
		fadeScreen.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		fadeScreen.enabled = true;
		FadeToClear ();
		isEnd = false;
	}

	void Update () {
		//VictoryScript will set isEnd to true when the player is at the end
		if (isEnd == true) {
			time = 0f;
			//wait to make the fade feel better
			Invoke("FadeToBlack", 3f);
			isEnd = false;
		}
	}

	/*
	 * A method to make the screen fade from black to clear
	 * Method calls iteself recursively until the view is clear.
	 * Time is stored as a variable so that the fade will work correctly.
	 */
	void FadeToClear(){
		Color temp = fadeScreen.color;
		temp.a = time;
		fadeScreen.color = temp;
		time -=.01f;
		if (temp.a == 0f) {
			return;
		}
		Invoke ("FadeToClear", .01f);
	}

	/*
	 * A method to make the screen fade from clear to black.
	 * Method calls iteself recursively until the view is black.
	 */
	void FadeToBlack(){
		Color temp = fadeScreen.color;
		temp.a = time;
		fadeScreen.color = temp;
		time += .015f;
		if (temp.a == 1f) {
			return;
		}
		Invoke ("FadeToBlack", .02f);
	}
}
