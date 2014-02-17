using UnityEngine;
using System.Collections;

public class CrossHairScript : MonoBehaviour {
	private static CrossHairScript instance;
	private Transform firePoint;
	private Camera cam;
	// Use this for initialization
	void Awake () {
		instance = this;
		cam = GameObject.Find("Player").GetComponentInChildren<Camera>();
	}

	void LateUpdate(){
		if(firePoint==null)return;

		Vector3 fwd = firePoint.forward;
		RaycastHit hit;
		if(Physics.Raycast(firePoint.position,fwd,out hit)){
//			if(hit.collider==null)return;
//			if(hit.point==null)return;
			transform.position = cam.WorldToViewportPoint(hit.point);
		}
	}

	
	public static void updateCrosshair(Transform firePoint){
		instance.firePoint = firePoint;
	}
}
