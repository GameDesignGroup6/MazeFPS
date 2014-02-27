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

	private bool isEnd = false;
	public GridScript grid;//so that a gameObject with GridScript attached can be attached
	private GameObject playerObj;

	void Start(){
		debugMarker.transform.position = new Vector3(grid.Victory.x,2f,grid.Victory.z);
		say ("Level "+(Application.loadedLevel+1));
		playerObj = GameObject.Find ("Player");
		Invoke ("moveEntrance", 0);
	}

	void moveEntrance(){
		grid.ReturnEntrance().rigidbody.AddForce(0,-100,0);
		StopEntrance ();
	}

	void StopEntrance(){

		if (grid.ReturnEntrance ().position.y <= 0.05) {
			grid.ReturnEntrance().position = new Vector3(0,0,0);
			grid.ReturnEntrance().rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			return;
		}
		Invoke ("StopEntrance", 0f);
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
		if (Mathf.Abs(playerPos.x-grid.Victory.x)<1 && Mathf.Abs(playerPos.z-grid.Victory.z)<1 && (grid.Victory.x!=0 || grid.Victory.z!=0) && !isEnd){
			Debug.Log("player reached the end");
			grid.ReturnExit().rigidbody.AddForce(0,-100,0);
			Transform player = GameObject.Find("Player").transform;
			player.GetComponent<CharacterMotor>().canControl = false;
			player.GetComponent<FadeInOut>().isEnd = true;
			Invoke("Victory",6f);
			isEnd = true;
		}
		//why does everyone hate FixedUpdate?	
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.F11))
			debugMarker.renderer.enabled = !debugMarker.renderer.enabled;
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
	}
}