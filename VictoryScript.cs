using UnityEngine;
using System.Collections;

/**
 * This script checks if the player has reached the end of the maze
 * and moves the player to the next level.
 * Also an object that has GridScript attached should be attached to this script
 *
 */
public class VictoryScript:MonoBehaviour
{
	public GUIText text;
	private int maxFade = 150;
	private int fade = 0;
	public AnimationCurve fadeCurve;
	public GameObject debugMarker;
	private bool isEnd = false;
	/**a variable so that the victory square can be referenced*/
	public GridScript grid;
	/**a variable so that the player can be referenced*/
	private GameObject playerObj;

	/**
	 *finds the player and the location of the victory square
	 */
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
	 *Also, victory check is ignored if the victory square is at (0,0), which means the maze is not complete yet
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
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.F11))
			debugMarker.renderer.enabled = !debugMarker.renderer.enabled;
	}
	/**
	 *Loads the scene for the next level
	 *If there aren't more levels to load, a text saying that the player won appears
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