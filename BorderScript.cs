using UnityEngine;
using System.Collections;

/**
 * @author Austin Hacker
 * A class to encapsulate a rectangular grid of size Size;
 * Y-position assumed to be 0.
*/

public class BorderScript : MonoBehaviour {

	public Transform BorderPrefab;
	public Vector3 Size;
	public Transform[] Border;
	
	void Start () {
		CreateBorder ();
		CreateTunnel(new Vector3(0,16,0));
	}

	void CreateBorder(){
		//this is a regular array rather than a 2d array to save space
		Border = new Transform[(int)(2*((int)Size.x+2)+2*(Size.z+2))];

		//creates all squares along two parallel walls
		for (int x = 0; x < Size.x+2; x++) {
			
			Transform newCell;
			newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (4*(x - 1), 4, -4), Quaternion.identity);
			newCell.name = string.Format ("({0},0,{1})", 4*(x - 1), -4);
			newCell.parent = transform;
			Border [x] = newCell;
	
			newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (4*(x - 1), 4, 4*Size.z), Quaternion.identity);
			newCell.name = string.Format ("({0},0,{1})", 4*(x - 1), 4*Size.z);
			newCell.parent = transform;
			Border [x + (int)Size.x+2] = newCell;

		}
		//creates all remaining squares along last two walls
		for (int z = 1; z < Size.z+1; z++){
			Transform newCell;
			newCell = (Transform)Instantiate(BorderPrefab, new Vector3(-4, 4, 4*(z-1)), Quaternion.identity);
			newCell.name = string.Format("({0},0,{1})", -4, 4*(z-1));
			newCell.parent = transform;
			Border[z + 2*(int)Size.x+2] = newCell;

			newCell = (Transform)Instantiate(BorderPrefab, new Vector3(4*Size.x, 4, 4*(z-1)), Quaternion.identity);
			newCell.name = string.Format("({0},0,{1})",4*Size.x, 4*(z-1));
			newCell.parent = transform;
			Border[z + 2*((int)Size.x+2) + (int)Size.z+2] = newCell;
		}
	}

	/*
	 * A method to create a tunnel around a 4x4 square size, three squares long.
	 * This method is used to create a border around an elevator.
	 * position takes the position of the initial square.
	 */
	public void CreateTunnel(Vector3 position){
		// a for loop for each circle of cubes
		for(int j = 0; j < 3; j++){
			//generates three squares
			for (int i = -1; i < 2; i++) {
				//an incredibly nasty Instiantiate call for a border square
				Transform newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (4 * i + position.x, position.y-4*j + .001f, position.z-4), Quaternion.identity);
				newCell.name = string.Format("({0},{1},{2})",4 * i + position.x, position.y-4*j, position.z-4);
				newCell.parent = transform;
			}
			//generates another three squares parallel to the first three
			for (int i = -1; i < 2; i++) {
				Transform newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (4 * i + position.x, position.y-4*j + .001f, position.z+4), Quaternion.identity);
				newCell.name = string.Format("({0},{1},{2})",4 * i + position.x, position.y-4*j, position.z+4);
				newCell.parent = transform;
			}
			//fills in the two remaining squares
			Transform left = (Transform)Instantiate (BorderPrefab, new Vector3 (position.x-4, position.y-4*j + .001f, position.z), Quaternion.identity);
			Transform right = (Transform)Instantiate (BorderPrefab, new Vector3 (position.x+4, position.y-4*j + .001f, position.z), Quaternion.identity);
			left.parent = transform;
			right.parent = transform;
		}
	}
}
