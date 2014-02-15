using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

/**
 * Creates a grid of specified dimensions and generates a procedural maze using a
 * modified form of Prim's Algorithm.
 * @author Timothy Sesler
 * @author tds45
 * @date 4 February 2014
 * 
 * Adapted from work provided online by Austin Takechi 
 * Contact: MinoruTono@Gmail.com
 */ 
public class GridScript : MonoBehaviour {
	
	public Transform CellPrefab;
	public Vector3 Size;
	public Transform[,] Grid;
	
	// Use this for initialization
	void Start () {
		CreateGrid();
		SetRandomNumbers();
		SetAdjacents();
		SetStart();
		FindNext();
		FindExit ();
	}
	
	// Creates the grid by instantiating provided cell prefabs.
	void CreateGrid () {
		Grid = new Transform[(int)Size.x,(int)Size.z];
		
		// Places the cells and names them according to their coordinates in the grid.
		for (int x = 0; x < Size.x; x++) {
			for (int z = 0; z < Size.z; z++) {
				Transform newCell;
				newCell = (Transform)Instantiate(CellPrefab, new Vector3(x, 0, z), Quaternion.identity);
				newCell.name = string.Format("({0},0,{1})", x, z);
				newCell.parent = transform;
				newCell.GetComponent<CellScript>().Position = new Vector3(x, 0, z);
				Grid[x,z] = newCell;
			}
		}
		// Centers the camera on the maze.
		// Feel free to adjust this as needed.
		//Camera.main.transform.position = Grid[(int)(Size.x / 2f),(int)(Size.z / 2f)].position + Vector3.up * 20f;
		//Camera.main.orthographicSize = Mathf.Max(Size.x * 0.75f, Size.z * 0.7f);
	}


	// Sets a random weight to each cell.
	void SetRandomNumbers () {
		foreach (Transform child in transform) {
			int weight = Random.Range(0,10);
			child.GetComponentInChildren<TextMesh>().text = weight.ToString();
			child.GetComponent<CellScript>().Weight = weight;
		}
	}
	
	// Determines the adjacent cells of each cell in the grid.
	void SetAdjacents () {
		for(int x = 0; x < Size.x; x++){
			for (int z = 0; z < Size.z; z++) {
				Transform cell;
				cell = Grid[x,z];
				CellScript cScript = cell.GetComponent<CellScript>();
				
				if (x - 1 >= 0) {
					cScript.Adjacents.Add(Grid[x - 1, z]);
				}
				if (x + 1 < Size.x) {
					cScript.Adjacents.Add(Grid[x + 1, z]);
				}
				if (z - 1 >= 0) {
					cScript.Adjacents.Add(Grid[x, z - 1]);
				}
				if (z + 1 < Size.z) {
					cScript.Adjacents.Add(Grid[x, z + 1]);
				}
				
				cScript.Adjacents.Sort(SortByLowestWeight);
			}
		}
	}
	
	// Sorts the weights of adjacent cells.
	// Check the link for more info on custom comparators and sorting.
	// http://msdn.microsoft.com/en-us/library/0e743hdt.aspx
	int SortByLowestWeight (Transform inputA, Transform inputB) {
		int a = inputA.GetComponent<CellScript>().Weight;
		int b = inputB.GetComponent<CellScript>().Weight;
		return a.CompareTo(b);
	}
	
	/*********************************************************************
	 * Everything after this point pertains to generating the actual maze.
	 * Look at the Wikipedia page for more info on Prim's Algorithm.
	 * http://en.wikipedia.org/wiki/Prim%27s_algorithm
	 ********************************************************************/ 
	public List<Transform> PathCells;			// The cells in the path through the grid.
	public List<List<Transform>> AdjSet;		// A list of lists representing available adjacent cells.
	/** Here is the structure:
	 *  AdjSet{
	 * 		[ 0 ] is a list of all the cells
	 *      that have a weight of 0, and are
	 *      adjacent to the cells in the path
	 *      [ 1 ] is a list of all the cells
	 *      that have a weight of 1, and are
	 * 		adjacent to the cells in the path
	 *      ...
	 *      [ 9 ] is a list of all the cells
	 *      that have a weight of 9, and are
	 *      adjacent to the cells in the path
	 * 	}
	 *
	 * Note: Multiple entries of the same cell
	 * will not appear as duplicates.
	 * (Some adjacent cells will be next to
	 * two or three or four other path cells).
	 * They are only recorded in the AdjSet once.
	 */  
	
	// Initializes the sets and the starting cell.
	void SetStart () {
		PathCells = new List<Transform>();
		AdjSet = new List<List<Transform>>();
		
		for (int i = 0; i < 10; i++) {
			AdjSet.Add(new List<Transform>());	
		}
		
		Grid[0, 0].renderer.material.color = Color.green;
		AddToSet(Grid[0, 0]);
	}
	
	// Adds a cell to the set of visited cells.
	void AddToSet (Transform cellToAdd) {
		PathCells.Add(cellToAdd);
		
		foreach (Transform adj in cellToAdd.GetComponent<CellScript>().Adjacents) {
			adj.GetComponent<CellScript>().AdjacentsOpened++;
			
			if (!PathCells.Contains(adj) && !(AdjSet[adj.GetComponent<CellScript>().Weight].Contains(adj))) {
				AdjSet[adj.GetComponent<CellScript>().Weight].Add(adj);
			}
		}
	}
	
	// Determines the next cell to be visited.
	void FindNext () {
		Transform next;
		
		do {
			bool isEmpty = true;
			int lowestList = 0;
			
			// We loop through each sub-list in the AdjSet list of lists, until we find one with a count of more than 0.
			// If there are more than 0 items in the sub-list, it is not empty.
			// We've found the lowest sub-list, so there is no need to continue searching.
			for (int i = 0; i < 10; i++) {
				lowestList = i;
				
				if (AdjSet[i].Count > 0) {
					isEmpty = false;
					break;
				}
			}
			
			// The maze is complete.
			if (isEmpty) { 
				//Debug.Log("Generation completed in " + Time.timeSinceLevelLoad + " seconds."); 
				//CancelInvoke("FindNext");
				//PathCells[PathCells.Count - 1].renderer.material.color = Color.red;
				
				foreach (Transform cell in Grid) {
					// Removes displayed weight
					cell.GetComponentInChildren<TextMesh>().renderer.enabled = false;
					
					if (!PathCells.Contains(cell)) {
						// HINT: Try something here to make the maze 3D
						//cell.renderer.material.color = Color.black;
						cell.position = cell.position + Vector3.up;
					}
					//createChests();	
				}
				return;
			}
			// If we did not finish, then:
			// 1. Use the smallest sub-list in AdjSet as found earlier with the lowestList variable.
			// 2. With that smallest sub-list, take the first element in that list, and use it as the 'next'.
			next = AdjSet[lowestList][0];
			// Since we do not want the same cell in both AdjSet and Set, remove this 'next' variable from AdjSet.
			AdjSet[lowestList].Remove(next);
		} while (next.GetComponent<CellScript>().AdjacentsOpened >= 3);	// This keeps the walls in the grid, otherwise Prim's Algorithm would just visit every cell
		
		// The 'next' transform's material color becomes white.
		next.renderer.material.color = Color.white;
		// We add this 'next' transform to the Set our function.
		AddToSet(next);

		// Recursively call this function as soon as it finishes.
		FindNext ();
		// check if the space is a dead end
		int isDeadEnd = 1;
		foreach(Transform cell in next.GetComponent<CellScript>().Adjacents){
			if(cell.position.y == 1)continue;
			isDeadEnd -= 1;
		}
		// generate special squares
		if(isDeadEnd == 0) {
			int chance = Random.Range(0,10);
			if (chance < 6) next.renderer.material.color = Color.blue;
			if (chance >= 8) next.renderer.material.color = Color.yellow;
		}

	}

	void FindExit (){
		int pos = (int)Random.Range(0, Size.x-1);
		if (Grid[pos,(int)Size.z-1].position.y == 0f){
			Grid[pos,(int)Size.z-1].renderer.material.color = Color.red;
			return;
		}
		FindExit ();
	}


	// Called once per frame.
	void Update() {
		
		// Pressing 'F1' will generate a new maze.
		if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel(0);	
		}
	}
}
