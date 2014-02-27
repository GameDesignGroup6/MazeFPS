using UnityEngine;
using System.Collections;

public class BorderScript : MonoBehaviour {

	public Transform BorderPrefab;
	public Vector3 Size;
	public Transform[] Border;

	// Use this for initialization
	void Start () {
		CreateBorder ();
		CreateTunnel(new Vector3(0,16,0));
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

	public void CreateTunnel(Vector3 position){
		for(int j = 0; j < 3; j++){
		for (int i = -1; i < 2; i++) {
			Transform newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (4 * i + position.x, position.y-4*j, position.z-4), Quaternion.identity);
				newCell.name = string.Format("({0},{1},{2})",4 * i + position.x, position.y-4*j, position.z-4);
			newCell.parent = transform;
		}
		for (int i = -1; i < 2; i++) {
			Transform newCell = (Transform)Instantiate (BorderPrefab, new Vector3 (4 * i + position.x, position.y-4*j, position.z+4), Quaternion.identity);
				newCell.name = string.Format("({0},{1},{2})",4 * i + position.x, position.y-4*j, position.z+4);
			newCell.parent = transform;
		}
			Transform left = (Transform)Instantiate (BorderPrefab, new Vector3 (position.x-4, position.y-4*j, position.z), Quaternion.identity);
			Transform right = (Transform)Instantiate (BorderPrefab, new Vector3 (position.x+4, position.y-4*j, position.z), Quaternion.identity);
			left.parent = transform;
			right.parent = transform;
		}
	}
}
