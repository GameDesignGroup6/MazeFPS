using UnityEngine;
using System.Collections;

public class BorderMerger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("MergeBorder",.5f);
	}
	
	void MergeBorder(){
		
		MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = (meshFilters[i].transform.localToWorldMatrix);
			meshFilters[i].gameObject.SetActive(false);
			i++;
		}
		Transform mainWall = transform.GetChild (0);
		mainWall.GetComponent<MeshFilter>().mesh = new Mesh();
		mainWall.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		mainWall.gameObject.SetActive(true);
		mainWall.position = new Vector3 (0, 0, 0);
		mainWall.localScale = new Vector3 (1, 1, 1);
	}
}
