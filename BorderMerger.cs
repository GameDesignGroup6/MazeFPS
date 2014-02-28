using UnityEngine;
using System.Collections;

/**
 * @author Austin Hacker
 * A class to merge meshes of transforms parented by a given empty GameObject.
 * Merging all meshes is used to increase overall performance and reduce the chance of clipping.
 * This class should only be attached to a gameObject whose mesh is independent
 * of it's children or those who doesn't have meshes.
*/

public class BorderMerger : MonoBehaviour {

	public Transform mainWall;
	
	void Start () {
		//Invoking ensures that the children have all been created.
		Invoke ("MergeBorder",.5f);
	}

	/*
	 * The method that merges the meshes.
	 * This code is based off of code from unity's script reference for
	 * Mesh.CombineMeshes.
	 */
	void MergeBorder(){

		//first, create an array of all meshes
		MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		//use Unity's CombineInstance so Mesh.CombineMeshes may be called appropriately
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = (meshFilters[i].transform.localToWorldMatrix);
			meshFilters[i].gameObject.SetActive(false); //turn off old meshes
			i++;
		}
		//create new, combined mesh and attach to a child since the parent is not a transform
		Transform mainWall = transform.GetChild (0);
		Mesh OneTrueMesh = new Mesh ();
		mainWall.GetComponent<MeshFilter>().mesh = OneTrueMesh;
		mainWall.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		mainWall.gameObject.SetActive(true);

		//make sure the new mesh is the correct size and position
		mainWall.position = new Vector3 (0, 0, 0);
		mainWall.localScale = new Vector3 (1, 1, 1);
		mainWall.gameObject.AddComponent (typeof(MeshCollider));
		this.mainWall = mainWall; //store the child so VictoryScript may reference it
	}
}
