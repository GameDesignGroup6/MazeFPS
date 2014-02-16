using UnityEngine;
using System.Collections;

public class CeilingScript : MonoBehaviour {

	public Transform CeilingPrefab;
	public Vector3 Size;
	public bool visible;

	// Use this for initialization
	void Start () {	
		CreateCeiling ();
	}

	void CreateCeiling(){
		
		Transform ceiling;
		ceiling = (Transform)Instantiate(CeilingPrefab, new Vector3(2*Size.x, 6f, 2*Size.z),CeilingPrefab.rotation);
		ceiling.parent = transform;
		ceiling.localScale = new Vector3 ((4*Size.x+5)/10f, 1f, (4*Size.z+5)/10f);
		//ceiling.localRotation = Quaternion.AngleAxis (180, new Vector3 (1, 0, 0));//I made the ceiling prefab upside-down instead
		ceiling.renderer.enabled = visible;//for other levels that are indoors
	}
}
