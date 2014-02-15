using UnityEngine;
using System.Collections;

public class BorderScript : MonoBehaviour {

	public Transform BorderPrefab;
	public Vector3 Size;
	public Transform[,] Border;

	// Use this for initialization
	void Start () {
		CreateBorder ();
	}

	void CreateBorder(){
		Border = new Transform[(int)Size.x+2, (int)Size.z+2];
		
		for (int x = 0; x < Size.x+2; x++) {
			
			Transform newCell;
			newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (x - 1, 1, -1), Quaternion.identity);
			newCell.name = string.Format ("({0},0,{1})", x - 1, -1);
			newCell.parent = transform;
			Border [x, 0] = newCell;
	
			newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (x - 1, 1, Size.z), Quaternion.identity);
			newCell.name = string.Format ("({0},0,{1})", x - 1, Size.z);
			newCell.parent = transform;
			Border [x, (int)Size.z] = newCell;

		}
		for (int z = 1; z < Size.z+1; z++){
			Transform newCell;
			newCell = (Transform)Instantiate(BorderPrefab, new Vector3(-1, 1, z-1), Quaternion.identity);
			newCell.name = string.Format("({0},0,{1})", -1, z-1);
			newCell.parent = transform;
			Border[0,z] = newCell;

			newCell = (Transform)Instantiate(BorderPrefab, new Vector3(Size.x, 1, z-1), Quaternion.identity);
			newCell.name = string.Format("({0},0,{1})",Size.x, z-1);
			newCell.parent = transform;
			Border[(int)Size.x,z] = newCell;
			
		}
	}
}
