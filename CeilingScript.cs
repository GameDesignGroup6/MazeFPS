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
		ceiling = (Transform)Instantiate(CeilingPrefab, new Vector3(2*Size.x, 6f, 2*Size.z), Quaternion.identity);
		ceiling.parent = transform;
		ceiling.localScale = new Vector3 ((4*Size.x+5)/10f, 1f, (4*Size.z+5)/10f);
		ceiling.localRotation = Quaternion.AngleAxis (180, new Vector3 (1, 0, 0));
		ceiling.renderer.enabled = true;
	}
}
