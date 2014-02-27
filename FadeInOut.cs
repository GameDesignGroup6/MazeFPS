using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

	public GUITexture fadeScreen;
	public bool isEnd;
	private float time;

	// Use this for initialization
	void Start () {
		time = 1f;
		fadeScreen.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		fadeScreen.enabled = true;
		FadeToClear ();
		isEnd = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnd == true) {
			time = 0f;
			Invoke("FadeToBlack", 3f);
			isEnd = false;
		}
	}

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
