using UnityEngine;
using System.Collections;

/**
 * This script should be attached to an empty object
 * Also an object that has GridScript attached should be attached to this script
 * 
 * This script requires the following behavrios from GridScript
 * public Vector3 Victory; in the variable declarations
 * 
 * a method called
 * 	void CheckGridSize(){
		if (VictoryScript.gridSize.x != 0 && VictoryScript.gridSize.z != 0){
			Size= VictoryScript.gridSize;
		}
	}
 *
 *This line of code to record where the finish square is at
 *Victory=PathCells[PathCells.Count-1].position;
 */
public class VictoryScript:MonoBehaviour
{
	public GUIText text;
	private int maxFade = 150;
	private int fade = 0;
	public AnimationCurve fadeCurve;
	public GameObject debugMarker;


	public GridScript grid;//so that a gameObject with GridScript attached can be attached
	private GameObject playerObj;
	void Start(){
		debugMarker.transform.position = new Vector3(grid.Victory.x,2f,grid.Victory.z);
		say ("Level "+(Application.loadedLevel+1));
		playerObj = GameObject.Find ("Player");
		//DontDestroyOnLoad(this);
	}

	public void say(string message){
		text.enabled = true;
		text.text = message;
		fade = maxFade;
		float percent = ((float)fade)/((float)maxFade);
		Color color = text.color;
		color.a = fadeCurve.Evaluate(percent);
		text.color = color;
	}
	
	
	/**
	 *Uses the static Vector3 var from GridScript to check the victory condition
	 *Has leniency of .5f since it is hard for the character to be at the exact center of the victory square
	 */
	void FixedUpdate(){
		if(fade>0){
			fade--;
			float percent = ((float)fade)/((float)maxFade);
			Color color = text.color;
			color.a = fadeCurve.Evaluate(percent);
			text.color = color;
		}else{
			text.enabled = false;
		}

		if(Player.Dead){
			say("Game Over");
			return;
		}


		var playerPos = playerObj.transform.position;
		//Debug.Log ("Player is at" + playerPos);
		if (Mathf.Abs(playerPos.x-grid.Victory.x)<1 && Mathf.Abs(playerPos.z-grid.Victory.z)<1 && (grid.Victory.x!=0 || grid.Victory.z!=0)){
			Debug.Log("player reached the end");
			Victory();

		}
		//why does everyone hate FixedUpdate?
//		StartCoroutine(CheckVic());
		
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.F11))
			debugMarker.renderer.enabled = !debugMarker.renderer.enabled;
//		if(Input.GetKeyDown (KeyCode.F12))
//			debugMarker.renderer.enabled = false;
	}
	/**
	 *Loads the scene for the next level
	 */
	void Victory(){
		int nextLevel = Application.loadedLevel+1;
		if(nextLevel==Application.levelCount){
			say("You Win");
			return;
		}
		Application.LoadLevel(nextLevel);
//		gridSize= new Vector3(grid.Size.x+10,0,grid.Size.z+10);
//		Application.LoadLevel(0);
	}
}