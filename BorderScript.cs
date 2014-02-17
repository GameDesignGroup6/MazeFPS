using UnityEngine;
using System.Collections;

public class BorderScript : MonoBehaviour {

	public Transform BorderPrefab;
	public Vector3 Size;
	public Transform[] Border;

	// Use this for initialization
	void Start () {
		CreateBorder ();
	}

	void CreateBorder(){
		Border = new Transform[(int)(2*((int)Size.x+2)+2*(Size.z+2))];
		
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
}
