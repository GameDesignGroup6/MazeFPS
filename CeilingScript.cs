using UnityEngine;
using System.Collections;

/**
 * @author Austin Hacker
 * A class to generate a plane of a given size
 * This class is used to create a ceiling above our maze
*/

public class CeilingScript : MonoBehaviour {

	public Transform CeilingPrefab;
	public Vector3 Size;
	private Transform ceiling;

	// Use this for initialization
	void Start () {	
		CreateCeiling ();
	}

	void CreateCeiling(){
		
		Transform ceiling;
		ceiling = (Transform)Instantiate(CeilingPrefab, new Vector3(2*Size.x, 6f, 2*Size.z), Quaternion.identity);
		ceiling.parent = transform;
		//this scales the ceiling to match the size of the maze
		ceiling.localScale = new Vector3 ((4*Size.x+5)/10f, 1f, (4*Size.z+5)/10f);
		//this rotates the plane so that it is visible from within the maze
		ceiling.localRotation = Quaternion.AngleAxis (180, new Vector3 (1, 0, 0));
		ceiling.renderer.enabled = true;
		this.ceiling = ceiling; //store the ceiling so VictoryScript may access it
	}
}
