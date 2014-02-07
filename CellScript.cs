using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * The variables of a single maze cell.
 * @author Timothy Sesler
 * @author tds45
 * @date 4 February 2014
 * 
 * Adapted from work provided online by Austin Takechi 
 * Contact: MinoruTono@Gmail.com
 */ 
public class CellScript : MonoBehaviour {
	
	public List<Transform> Adjacents;
	public Vector3 Position;
	public int Weight;
	public int AdjacentsOpened = 0;
}