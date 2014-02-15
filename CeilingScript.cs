using UnityEngine;
using System.Collections;

public class CeilingScript : MonoBehaviour {

	public Transform CeilingPrefab;
	public Vector3 Size;

	// Use this for initialization
	void Start () {	
		CreateCeiling ();
	}

	void CreateCeiling(){
		
		Transform ceiling;
		ceiling = (Transform)Instantiate(CeilingPrefab, new Vector3(Size.x/2, 1.5f, Size.z/2), Quaternion.identity);
		ceiling.parent = transform;
		ceiling.localScale = new Vector3 ((Size.x+1)/10f, 1f, (Size.z+1)/10f);
		ceiling.renderer.enabled = false;
	}
}
